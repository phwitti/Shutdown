
//

#include <Windows.h>
#include <ShObjIdl.h>
#include <propvarutil.h>
#include <propkey.h>
#include <iostream>

//

#include "exception.hpp"
#include "jumplist.hpp"

//

namespace pw
{
    void JumpList::commit()
    {
        commit(Application::getCurrent());
    }

    void JumpList::commit(const Application &_application)
    {
        ICustomDestinationList* destinationList;

        if (FAILED(CoCreateInstance(CLSID_DestinationList, nullptr, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&destinationList))))
        {
            throw Win32Exception();
        }

        //

        if (_application.getId() != std::wstring())
        {
            destinationList->SetAppID(_application.getId().c_str());
        }

        //

        IObjectArray* objectArray;
        UINT c_uMinObjectArraySlots;
        if (FAILED(destinationList->BeginList(&c_uMinObjectArraySlots, IID_PPV_ARGS(&objectArray))))
        {
            throw Win32Exception();
        }

        //

        IObjectCollection* shellObjectCollection;
        if (FAILED(CoCreateInstance(CLSID_EnumerableObjectCollection, nullptr, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&shellObjectCollection))))
        {
            throw Win32Exception();
        }

        //

        for (JumpTask &jumpTask : jumpTasks)
        {
            IShellLinkW* shellLink;

            if (FAILED(CoCreateInstance(CLSID_ShellLink, nullptr, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&shellLink))))
            {
                throw Win32Exception();
            }

            shellLink->SetArguments(jumpTask.arguments.c_str());
            shellLink->SetDescription(jumpTask.description.c_str());
            shellLink->SetIconLocation(jumpTask.iconResourcePath.c_str(), jumpTask.iconResourceIndex);
            shellLink->SetPath(jumpTask.applicationPath.c_str());

            IPropertyStore* propertyStore;
            if (FAILED(shellLink->QueryInterface(&propertyStore)))
            {
                throw Win32Exception();
            }

            PROPVARIANT propertyValue;
            InitPropVariantFromString(jumpTask.title.c_str(), &propertyValue);
            propertyStore->SetValue(PKEY_Title, propertyValue);
            propertyStore->Commit();
            PropVariantClear(&propertyValue);
            propertyStore->Release();

            shellObjectCollection->AddObject(shellLink);
            shellLink->Release();
        }

        //

        // Add the specified user task to the Task category of a Jump List
        IObjectArray* userTask;
        if (FAILED(shellObjectCollection->QueryInterface(&userTask)))
        {
            throw Win32Exception();
        }

        destinationList->AddUserTasks(userTask);
        userTask->Release();

        //

        shellObjectCollection->Release();
        if (FAILED(destinationList->CommitList()))
        {
            throw Win32Exception();
        }
        destinationList->Release();
    }
}
