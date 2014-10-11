#ifndef PROGRAMM_HPP
#define PROGRAMM_HPP

//

#include <string>
#include <vector>

//

namespace pwshutdown
{
    class Program
    {

    public:

        void run(std::vector<std::wstring> _arArgs);
        
    private:

        void createJumpList();

        //

    private:

        // Command line arguments to start appropriate behaviour
        const std::wstring c_sShutdownArgument = L"-s";
        const std::wstring c_sRebootArgument = L"-r";
        const std::wstring c_sLogoffArgument = L"-l";
        const std::wstring c_sStandbyArgument = L"-h";

        // Use english default values, if localisation fails
        const std::wstring c_sShutdownDefaultTitle = L"Shutdown";
        const std::wstring c_sRebootDefaultTitle = L"Restart";
        const std::wstring c_sLogoffDefaultTitle = L"Log off";
        const std::wstring c_sStandbyDefaultTitle = L"Sleep";

        const int c_iShutdownTitleStringId = 3013;
        const int c_iRebootTitleStringId = 3016;
        const int c_iLogoffTitleStringId = 3034;
        const int c_iStandbyTitleStringId = 3019;

        const int c_iShutdownDescriptionStringId = 3015;
        const int c_iRebootDescriptionStringId = 3018;
        const int c_iLogoffDescriptionStringId = 3035;
        const int c_iStandbyDescriptionStringId = 3021;

        // The library that contains the localized shutdown option strings
        const std::wstring c_sShutdownLocalizationLibrary = L"authui.dll.mui";

    };
}

//

#endif
