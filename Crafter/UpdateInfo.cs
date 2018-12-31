using System;
using System.Collections;

namespace Crafter
{
	[Serializable]
	public class UpdateInfo
	{
		public bool mEuro;

		public bool mUS;

		public bool mToA;

		public bool mDR;

		public bool mCAT;

		public bool mKillProcess;

		public bool mStopQual;

		public string mQualNum;

		public bool mSelling;

		public string mCurrentItem;

		public bool mStock;

		public string mVendorName;

		public string mItemSlot;

		public int mXPos;

		public int mYPos;

		public string mCraftKey;

		public bool mQuit;

		public string mDistance;

		public string mRegKey;

		public string mChatLog;

		public string mGamePath;

		public bool mSalvage;

		public string mVendorLoc;

		public string mForgeLoc;

		public int mQuality;

		public bool mDisableSound;

		public ArrayList mWaypoints;

		public UpdateInfo()
		{
		}
	}
}