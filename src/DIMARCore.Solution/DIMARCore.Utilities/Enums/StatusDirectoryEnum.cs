using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    /// <summary>
    /// Flags that control the behavior of the user account.
    /// </summary>
    public enum StatusDirectoryEnum
    {
        // 512	Enabled Account
        [Description("512 Enabled Account")]
        NORMAL_ACCOUNT = 512,

        //514 Disabled Account
        [Description("514 Disabled Account")]
        NORMAL_ACCOUNT_BLOCKED = 514,

        //544 Enabled, Password Not Required
        [Description("544 Enabled, Password Not Required")]
        Enabled_Password_Not_Required_544 = 544,

        // 546	Disabled, Password Not Required
        [Description("546 Disabled, Password Not Required")]
        Disabled_Password_Not_Required_546 = 546,

        // 66048 Enabled, Password Doesn't Expire
        [Description("66048	Enabled, Password Doesn't Expire")]
        Enabled_Password_Doesnt_Expire_66048 = 66048,

        // 66050 Disabled, Password Doesn't Expire
        [Description("66050	Disabled, Password Doesn't Expire")]
        Disabled_Password_Doesnt_Expire_66050 = 66050,

        // 66080 Enabled, Password Doesn't Expire & Not Required
        [Description("66080	Enabled, Password Doesn't Expire & Not Required")]
        Enabled_Password_Doesnt_Expire_And_Not_Required_66080 = 66080,

        // 66082 Disabled, Password Doesn't Expire & Not Required
        [Description("66082 Disabled, Password Doesn't Expire & Not Required")]
        Disabled_Password_Doesnt_Expire_And_Not_Required_66082 = 66082,

        // 262656 Enabled, Smartcard Required
        [Description("262656 Enabled, Smartcard Required")]
        Enabled_Smartcard_Required_262656 = 262656,

        // 262658 Disabled, Smartcard Required
        [Description("262658 Disabled, Smartcard Required")]
        Disabled_Smartcard_Required_262658 = 262658,

        // 262688 Enabled, Smartcard Required, Password Not Required
        [Description("262688 Enabled, Smartcard Required, Password Not Required")]
        Enabled_Smartcard_Required_Password_Not_Required_262688 = 262688,

        // 262690 Disabled, Smartcard Required, Password Not Required
        [Description("262690 Disabled, Smartcard Required, Password Not Required")]
        Disabled_Smartcard_Required_Password_Not_Required_262690 = 262690,

        // 328192 Enabled, Smartcard Required, Password Doesn't Expire
        [Description("328192 Enabled, Smartcard Required, Password Doesn't Expire")]
        Enabled_Smartcard_Required_Password_Doesnt_Expire_328192 = 328192,

        // 328194 Disabled, Smartcard Required, Password Doesn't Expire
        [Description("328194 Disabled, Smartcard Required, Password Doesn't Expire")]
        Disabled_Smartcard_Required_Password_Doesnt_Expire_328194 = 328194,

        // 328224 Enabled, Smartcard Required, Password Doesn't Expire & Not Required
        [Description("328224 Enabled, Smartcard Required, Password Doesn't Expire & Not Required")]
        Enabled_Smartcard_Required_Password_Doesnt_Expire_And_Not_Required_328224 = 328224,

        // 328226 Disabled, Smartcard Required, Password Doesn't Expire & Not Required
        [Description("328226 Disabled, Smartcard Required, Password Doesn't Expire & Not Required")]
        Disabled_Smartcard_Required_Password_Doesnt_Expire_And_Not_Required_328226 = 328226,

        // 8388608 Password Expired
        [Description("8388608 Password Expired")]
        Password_Expired_8388608 = 8388608,

        /// <summary>
        /// The account is currently locked out. 
        ///</summary>
        LOCKOUT = 0x00000010
    }
}