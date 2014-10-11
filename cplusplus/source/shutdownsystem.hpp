#ifndef SHUTDOWNSYSTEM_HPP
#define SHUTDOWNSYSTEM_HPP

//

#include <map>
#include <vector>

//

namespace pw
{
    class ShutdownSystem
    {

    public:
    
        enum class ShutdownForceType
        {
            DoNotForce = 0x00000000,
            Force = 0x00000004,
            ForceIfHung = 0x00000010
        };
        
        static void openShutdownWindow();
        static void shutdown(bool _useHybrid = false, ShutdownForceType _force = ShutdownForceType::DoNotForce);
        static void reboot(ShutdownForceType _force = ShutdownForceType::DoNotForce);
        static void logOff(ShutdownForceType _force = ShutdownForceType::DoNotForce);
        static void standby(ShutdownForceType _force = ShutdownForceType::DoNotForce);
        
    private:
    
        static bool shutdown(int _iFlag);

    };
}

//

#endif
