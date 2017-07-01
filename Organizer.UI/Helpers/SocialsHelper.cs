using System.Collections.Generic;

namespace Organizer.UI.Helpers
{
    public static class SocialsHelper
    {
        private static readonly List<string> _predefinedSocials;

        static SocialsHelper()
        {
            _predefinedSocials = new List<string>()
            {
                "Skype",
                "Vkontakte",
                "Facebook",
                "Twitter",
                "Instagram",
                "Viber",
                "WhatsApp",
                "Line",
                "Telegram",
                "Phone"
            };
        }

        public static List<string> GetPredefinedSocials()
        {
            return _predefinedSocials;
        }
    }
}