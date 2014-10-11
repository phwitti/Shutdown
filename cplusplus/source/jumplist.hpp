#ifndef JUMPLIST_HPP
#define JUMPLIST_HPP

//

#include <list>

//

#include "application.hpp"
#include "comobject.hpp"
#include "jumptask.hpp"

//

namespace pw
{
    class JumpList : public ComObject
    {

    public:
        
        void commit();
        void commit(const Application &_application);
        
        //
        
        std::list<JumpTask> jumpTasks;

    };
}

//

#endif
