using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;

namespace MC
{
    public class MPToken
    {
        public static bool IsWeixinUserHasAppRight(string openId)
        {
            try
            {
                UserApi.Info(MPBasicSetting.AppID, openId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
