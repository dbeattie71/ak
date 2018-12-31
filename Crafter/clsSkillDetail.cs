using System;

namespace Crafter
{
	public class clsSkillDetail
	{
		private string mSkillLevel;

		private string mItemName;

		private string mCraftKey;

		public string CraftKey
		{
			get
			{
				return this.mCraftKey;
			}
			set
			{
				this.mCraftKey = value;
			}
		}

		public string ItemName
		{
			get
			{
				return this.mItemName;
			}
			set
			{
				this.mItemName = value;
			}
		}

		public string SkillLevel
		{
			get
			{
				return this.mSkillLevel;
			}
			set
			{
				this.mSkillLevel = value;
			}
		}

		public clsSkillDetail()
		{
		}
	}
}