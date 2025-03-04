﻿using System;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace ArenaBell
{
	// Token: 0x02000013 RID: 19
	public class LordJob_Joinable_FightingMatch : LordJob_VoluntarilyJoinable
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00003354 File Offset: 0x00001554
		public LordJob_Joinable_FightingMatch()
		{
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000335E File Offset: 0x0000155E
		public LordJob_Joinable_FightingMatch(IntVec3 spot, Building_Bell bell)
		{
			this.spot = spot;
			this.bell = bell;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003378 File Offset: 0x00001578
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000338C File Offset: 0x0000158C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_FightingMatch lordToilFightingMatch = new LordToil_FightingMatch(this.spot, this.bell);
			stateGraph.AddToil(lordToilFightingMatch);
			LordToil_End lordToilEnd = new LordToil_End();
			stateGraph.AddToil(lordToilEnd);
			Transition transition = new Transition(lordToilFightingMatch, lordToilEnd, false, true);
			transition.AddTrigger(new Trigger_TickCondition(() => this.bell.currentState == Building_Bell.State.rest, 1));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000033FC File Offset: 0x000015FC
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			bool flag = this.IsInvited(p);
			float result;
			if (flag)
			{
				result = VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003428 File Offset: 0x00001628
		public virtual string GetReport()
		{
			return Translator.Translate("LordReportAttendingParty");
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003444 File Offset: 0x00001644
		private bool IsInvited(Pawn p)
		{
			bool flag = this.lord.faction != null;
			return flag && p.Faction == this.lord.faction;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003480 File Offset: 0x00001680
		public override void ExposeData()
		{
			Scribe_References.Look<Building_Bell>(ref this.bell, "bell", false);
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
		}

		// Token: 0x0400001B RID: 27
		private IntVec3 spot;

		// Token: 0x0400001C RID: 28
		private Building_Bell bell;
	}
}
