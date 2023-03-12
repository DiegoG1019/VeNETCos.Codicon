using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeNETCos.Codicon.Configuration.LanguageModules;

namespace VeNETCos.Codicon.Configuration;
public static class Language
{
    private static Lang curr;
    public static Lang CurrentLanguage
    {
        get => curr;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (curr == value) return;
            curr = value;
            LanguageChanged?.Invoke();
        }
    }

    public static ErrorsLang Errors => curr.Errors;

    static Language()
    {
        curr = new Lang(
            new ErrorsLang(
                InvalidPathError: "The entered path is invalid",
                UsernameNullError: "Username cannot be empty",
                InvalidUsernameError: "The provided username is invalid"
            ),
            new UserLogin(
                WelcomeMessage: "Welcome!",
                WelcomeMessageSmall: "Glad to ese you here.",
                LoginButton: "Login"
            )
        );
    }

    public static event Action? LanguageChanged;
}