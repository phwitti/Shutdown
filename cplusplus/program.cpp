
//

#include <string>
#include <vector>
#include <iostream>

//

#include "program.hpp"
#include "source\actionmap.hpp"
#include "source\application.hpp"
#include "source\dynamiclibrary.hpp"
#include "source\environment.hpp"
#include "source\jumplist.hpp"
#include "source\jumptask.hpp"
#include "source\path.hpp"
#include "source\shutdownsystem.hpp"

//

int wmain(int _iArgc, wchar_t *_arArgv[])
{
    pw::Application::initialize();

    try
    {
        pwshutdown::Program().run(std::vector<std::wstring>(_arArgv, _arArgv + _iArgc));
    }
    catch (std::exception &_exception)
    {
        std::cerr << _exception.what() << std::endl;
        throw;
    }
}


//

namespace pwshutdown
{
    void Program::run(std::vector<std::wstring> _arArgs)
    {
        // Create a map that associates the command line arguments with their appropriate behaviour
        pw::ActionMap<std::wstring> actionMap;
        actionMap.emplace(c_sShutdownArgument, [](){ pw::ShutdownSystem::shutdown(true); });
        actionMap.emplace(c_sRebootArgument,   [](){ pw::ShutdownSystem::reboot(); });
        actionMap.emplace(c_sLogoffArgument,   [](){ pw::ShutdownSystem::logOff(); });
        actionMap.emplace(c_sStandbyArgument,  [](){ pw::ShutdownSystem::standby(); });
    
        // There's no possibility to check, if the jumplist has already been created on a previous startup
        // -> Create new one
        createJumpList();

        // Try to find and start behaviour associated with the given command line arguments
        if (!actionMap.executeFirst(_arArgs))
        {
            // If no behaviour bound command line argument was found, open up the shutdown dialog window
            pw::ShutdownSystem::openShutdownWindow();
        }
    }

    void Program::createJumpList()
    {
        // Create a new jumplist
        pw::JumpList jumpList;

        // Get the current location of the executable
        std::wstring sCurrentExecutablePath = pw::Application::getCurrent().getFileName();

        // Try to load the localized system file that includes the shutdown option strings
        pw::DynamicLibrary libLocalizedShutdown;
        
        try
        {
            libLocalizedShutdown.load(pw::Path::combine({ pw::Environment::getSystemDirectory(), pw::Environment::getCurrentLocaleName(), c_sShutdownLocalizationLibrary }));
        }
        catch (...)
        {
            // Nothing to do
        }

        // Read the jumplist item titles from the localized system file "authui.dll.mui"
        std::wstring sShutdown = libLocalizedShutdown.loadString(c_iShutdownTitleStringId, c_sShutdownDefaultTitle);
        std::wstring sReboot = libLocalizedShutdown.loadString(c_iRebootTitleStringId, c_sRebootDefaultTitle);
        std::wstring sLogoff = libLocalizedShutdown.loadString(c_iLogoffTitleStringId, c_sLogoffDefaultTitle);
        std::wstring sStandby = libLocalizedShutdown.loadString(c_iStandbyTitleStringId, c_sStandbyDefaultTitle);

        std::wstring sShutdownDescription = libLocalizedShutdown.loadString(c_iShutdownDescriptionStringId, std::wstring());
        std::wstring sRebootDescription = libLocalizedShutdown.loadString(c_iRebootDescriptionStringId, std::wstring());
        std::wstring sLogoffDescription = libLocalizedShutdown.loadString(c_iLogoffDescriptionStringId, std::wstring());
        std::wstring sStandbyDescription = libLocalizedShutdown.loadString(c_iStandbyDescriptionStringId, std::wstring());

        // Create and add jumpitems to the jumplist
        jumpList.jumpTasks.push_back(pw::JumpTask(sShutdown, sCurrentExecutablePath, c_sShutdownArgument, sShutdownDescription, sCurrentExecutablePath, 3));
        jumpList.jumpTasks.push_back(pw::JumpTask(sReboot, sCurrentExecutablePath, c_sRebootArgument, sRebootDescription, sCurrentExecutablePath, 2));
        jumpList.jumpTasks.push_back(pw::JumpTask(sStandby, sCurrentExecutablePath, c_sStandbyArgument, sStandbyDescription, sCurrentExecutablePath, 4));
        jumpList.jumpTasks.push_back(pw::JumpTask(sLogoff, sCurrentExecutablePath, c_sLogoffArgument, sLogoffDescription, sCurrentExecutablePath, 1));

        // Push the changed jumplist to the shell
        jumpList.commit();
    }
}
