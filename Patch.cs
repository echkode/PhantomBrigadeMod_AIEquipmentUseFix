// Copyright (c) 2024 EchKode
// SPDX-License-Identifier: BSD-3-Clause

using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using PhantomBrigade.AI;
using PhantomBrigade.AI.Systems;
using PhantomBrigade.Data;

namespace EchKode.PBMods.AIEquipmentUseFix
{
	[HarmonyPatch]
	public static partial class Patch
	{
		[HarmonyPatch(typeof(CombatAIBehaviorInvokeSystem), "CollapseEquipmentUse")]
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> Caibis_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			var cm = new CodeMatcher(instructions, generator);
			var getActionFromTableMethodInfo = AccessTools.DeclaredMethod(typeof(AIUtility), nameof(AIUtility.GetActionFromTable));
			var getPaintedActionDurationMethodInfo = AccessTools.DeclaredMethod(typeof(DataHelperAction), nameof(DataHelperAction.GetPaintedActionDuration), new System.Type[]
			{
				typeof(string),
				typeof(PersistentEntity),
				typeof(EquipmentEntity),
			});
			var getActionFromTableMatch = new CodeMatch(OpCodes.Call, getActionFromTableMethodInfo);
			var getPaintedActionDurationMatch = new CodeMatch(OpCodes.Call, getPaintedActionDurationMethodInfo);
			var retMatch = new CodeMatch(OpCodes.Ret);
			var dupe = new CodeInstruction(OpCodes.Dup);
			var callGetPart = CodeInstruction.Call(typeof(Patch), nameof(GetPart));
			var callGetPaintedActionDuration = CodeInstruction.Call(typeof(DataHelperAction), nameof(DataHelperAction.GetPaintedActionDuration), new System.Type[]
			{
				typeof(DataContainerAction),
				typeof(PersistentEntity),
				typeof(EquipmentEntity),
			});

			cm.Start()
				.MatchEndForward(getActionFromTableMatch)
				.MatchEndForward(retMatch)
				.Advance(5);
			var loadActionData = cm.Instruction.Clone();
			cm.MatchStartForward(getPaintedActionDurationMatch)
				.Advance(-3)
				.SetInstructionAndAdvance(loadActionData)
				.Advance(1)
				.InsertAndAdvance(dupe)
				.InsertAndAdvance(loadActionData)
				.SetInstructionAndAdvance(callGetPart)
				.SetInstructionAndAdvance(callGetPaintedActionDuration);

			return cm.InstructionEnumeration();
		}

		public static EquipmentEntity GetPart(PersistentEntity unitPersistent, DataContainerAction actionData)
		{
			if (actionData.dataEquipment == null || !actionData.dataEquipment.partUsed)
			{
				return null;
			}
			return EquipmentUtility.GetPartInUnit(unitPersistent, actionData.dataEquipment.partSocket);
		}
	}
}
