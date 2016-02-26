//-----------------------------------------------------------------------------
// <auto-generated> 
//   This code was generated by a tool. 
// 
//   Changes to this file may cause incorrect behavior and will be lost if  
//   the code is regenerated.
//
//   Tool: AllJoynCodeGenerator.exe
//
//   This tool is located in the Windows 10 SDK and the Windows 10 AllJoyn 
//   Visual Studio Extension in the Visual Studio Gallery.  
//
//   The generated code should be packaged in a Windows 10 C++/CX Runtime  
//   Component which can be consumed in any UWP-supported language using 
//   APIs that are available in Windows.Devices.AllJoyn.
//
//   Using AllJoynCodeGenerator - Invoke the following command with a valid 
//   Introspection XML file and a writable output directory:
//     AllJoynCodeGenerator -i <INPUT XML FILE> -o <OUTPUT DIRECTORY>
// </auto-generated>
//-----------------------------------------------------------------------------
#pragma once

using namespace concurrency;

namespace com { namespace example { namespace SampleInterface {

ref class SampleInterfaceConsumer;

public ref class SampleInterfaceFrobateResult sealed
{
public:
    property int32 Status
    {
        int32 get() { return m_status; }
    internal:
        void set(_In_ int32 value) { m_status = value; }
    }

    property Platform::String^ Bar
    {
        Platform::String^ get() { return m_interfaceMemberBar; }
    internal:
        void set(_In_ Platform::String^ value) { m_interfaceMemberBar = value; }
    }
    property Windows::Foundation::Collections::IMap<uint32,Platform::String^>^ Baz
    {
        Windows::Foundation::Collections::IMap<uint32,Platform::String^>^ get() { return m_interfaceMemberBaz; }
    internal:
        void set(_In_ Windows::Foundation::Collections::IMap<uint32,Platform::String^>^ value) { m_interfaceMemberBaz = value; }
    }
    
    static SampleInterfaceFrobateResult^ CreateSuccessResult(_In_ Platform::String^ interfaceMemberBar, _In_ Windows::Foundation::Collections::IMap<uint32,Platform::String^>^ interfaceMemberBaz)
    {
        auto result = ref new SampleInterfaceFrobateResult();
        result->Status = Windows::Devices::AllJoyn::AllJoynStatus::Ok;
        result->Bar = interfaceMemberBar;
        result->Baz = interfaceMemberBaz;
        result->m_creationContext = Concurrency::task_continuation_context::use_current();
        return result;
    }
    
    static SampleInterfaceFrobateResult^ CreateFailureResult(_In_ int32 status)
    {
        auto result = ref new SampleInterfaceFrobateResult();
        result->Status = status;
        return result;
    }
internal:
    Concurrency::task_continuation_context m_creationContext = Concurrency::task_continuation_context::use_default();

private:
    int32 m_status;
    Platform::String^ m_interfaceMemberBar;
    Windows::Foundation::Collections::IMap<uint32,Platform::String^>^ m_interfaceMemberBaz;
};

public ref class SampleInterfaceBazifyResult sealed
{
public:
    property int32 Status
    {
        int32 get() { return m_status; }
    internal:
        void set(_In_ int32 value) { m_status = value; }
    }

    property Platform::Object^ Bar
    {
        Platform::Object^ get() { return m_interfaceMemberBar; }
    internal:
        void set(_In_ Platform::Object^ value) { m_interfaceMemberBar = value; }
    }
    
    static SampleInterfaceBazifyResult^ CreateSuccessResult(_In_ Platform::Object^ interfaceMemberBar)
    {
        auto result = ref new SampleInterfaceBazifyResult();
        result->Status = Windows::Devices::AllJoyn::AllJoynStatus::Ok;
        result->Bar = interfaceMemberBar;
        result->m_creationContext = Concurrency::task_continuation_context::use_current();
        return result;
    }
    
    static SampleInterfaceBazifyResult^ CreateFailureResult(_In_ int32 status)
    {
        auto result = ref new SampleInterfaceBazifyResult();
        result->Status = status;
        return result;
    }
internal:
    Concurrency::task_continuation_context m_creationContext = Concurrency::task_continuation_context::use_default();

private:
    int32 m_status;
    Platform::Object^ m_interfaceMemberBar;
};

public ref class SampleInterfaceMogrifyResult sealed
{
public:
    property int32 Status
    {
        int32 get() { return m_status; }
    internal:
        void set(_In_ int32 value) { m_status = value; }
    }

    
    static SampleInterfaceMogrifyResult^ CreateSuccessResult()
    {
        auto result = ref new SampleInterfaceMogrifyResult();
        result->Status = Windows::Devices::AllJoyn::AllJoynStatus::Ok;
        result->m_creationContext = Concurrency::task_continuation_context::use_current();
        return result;
    }
    
    static SampleInterfaceMogrifyResult^ CreateFailureResult(_In_ int32 status)
    {
        auto result = ref new SampleInterfaceMogrifyResult();
        result->Status = status;
        return result;
    }
internal:
    Concurrency::task_continuation_context m_creationContext = Concurrency::task_continuation_context::use_default();

private:
    int32 m_status;
};

public ref class SampleInterfaceJoinSessionResult sealed
{
public:
    property int32 Status
    {
        int32 get() { return m_status; }
    internal:
        void set(_In_ int32 value) { m_status = value; }
    }

    property SampleInterfaceConsumer^ Consumer
    {
        SampleInterfaceConsumer^ get() { return m_consumer; }
    internal:
        void set(_In_ SampleInterfaceConsumer^ value) { m_consumer = value; }
    };

private:
    int32 m_status;
    SampleInterfaceConsumer^ m_consumer;
};

public ref class SampleInterfaceGetBarResult sealed
{
public:
    property int32 Status
    {
        int32 get() { return m_status; }
    internal:
        void set(_In_ int32 value) { m_status = value; }
    }

    property byte Bar
    {
        byte get() { return m_value; }
    internal:
        void set(_In_ byte value) { m_value = value; }
    }

    static SampleInterfaceGetBarResult^ CreateSuccessResult(_In_ byte value)
    {
        auto result = ref new SampleInterfaceGetBarResult();
        result->Status = Windows::Devices::AllJoyn::AllJoynStatus::Ok;
        result->Bar = value;
        result->m_creationContext = Concurrency::task_continuation_context::use_current();
        return result;
    }

    static SampleInterfaceGetBarResult^ CreateFailureResult(_In_ int32 status)
    {
        auto result = ref new SampleInterfaceGetBarResult();
        result->Status = status;
        return result;
    }
internal:
    Concurrency::task_continuation_context m_creationContext = Concurrency::task_continuation_context::use_default();

private:
    int32 m_status;
    byte m_value;
};

public ref class SampleInterfaceSetBarResult sealed
{
public:
    property int32 Status
    {
        int32 get() { return m_status; }
    internal:
        void set(_In_ int32 value) { m_status = value; }
    }

    static SampleInterfaceSetBarResult^ CreateSuccessResult()
    {
        auto result = ref new SampleInterfaceSetBarResult();
        result->Status = Windows::Devices::AllJoyn::AllJoynStatus::Ok;
        result->m_creationContext = Concurrency::task_continuation_context::use_current();
        return result;
    }

    static SampleInterfaceSetBarResult^ CreateFailureResult(_In_ int32 status)
    {
        auto result = ref new SampleInterfaceSetBarResult();
        result->Status = status;
        return result;
    }
internal:
    Concurrency::task_continuation_context m_creationContext = Concurrency::task_continuation_context::use_default();

private:
    int32 m_status;
};

} } } 
