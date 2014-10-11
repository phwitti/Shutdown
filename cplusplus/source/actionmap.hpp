#ifndef ACTIONMAP_HPP
#define ACTIONMAP_HPP

//

#include <functional>
#include <map>
#include <vector>

//

namespace pw
{
    template<typename T>
    class ActionMap : public std::map<T, std::function<void(void)>>
    {

    public:
        
        bool execute(const T &_key) const;
        bool executeFirst(const std::vector<T> &_keys) const;
        bool executeAll(const std::vector<T> &_keys) const;

    };
}

//

#include "actionmap.tlh"

//

#endif
