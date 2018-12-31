using System;

namespace Crafter
{
	public class clsOrderDetail
	{
		private string mItemName;

		private string mMinQuality;

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

		public string MinQuality
		{
			get
			{
				return this.mMinQuality;
			}
			set
			{
				this.mMinQuality = value;
			}
		}

		public clsOrderDetail()
		{
		}
	}
}