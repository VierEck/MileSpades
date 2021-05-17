#if __linux__
	#define OS_PLATFORM_LINUX
#elif TARGET_OS_MAC
	#define OS_PLATFORM_MAC
#elif defined _WIN32 || defined _WIN64
	#define OS_PLATFORM_WINDOWS
	#include <Windows.h>
	#include <sstream>
	#include <VersionHelpers.h> // Requires windows 8.1 sdk at least
#endif

#include "VersionInfo.h"

std::string VersionInfo::GetVersionInfo() {
#if defined(OS_PLATFORM_LINUX)
	return std::string("Linux | DankSpades: github.com/yusufcardinal/openspades");
#elif defined(TARGET_OS_MAC)
	return std::string("Mac OS X | DankSpades: github.com/yusufcardinal/openspades");
#elif defined(OS_PLATFORM_WINDOWS)
	
	std::string windowsVersion;

	if (IsWindowsXPOrGreater() && !IsWindowsVistaOrGreater()) {
		windowsVersion = "Windows XP | DankSpades: github.com/yusufcardinal/openspades";
	} else if (IsWindowsVistaOrGreater() && !IsWindows7OrGreater()) {
		windowsVersion = "Windows Vista | DankSpades: github.com/yusufcardinal/openspades";
	} else if (IsWindows7OrGreater() && !IsWindows8OrGreater()) {
		windowsVersion = "Windows 7 | DankSpades: github.com/yusufcardinal/openspades";
	} else if (IsWindows8OrGreater() && !IsWindows8Point1OrGreater()) {
		windowsVersion = "Windows 10 | DankSpades: github.com/yusufcardinal/openspades";
	} else if (IsWindows8Point1OrGreater()) {
		windowsVersion = "Windows 8.1 | DankSpades: github.com/yusufcardinal/openspades";
	} else {
		// Default to Windows 10
		// See https://github.com/yvt/openspades/pull/528 for reason.
		windowsVersion = "Windows 10";
	}

	// Might be a greater version, but the new Microsoft
	// API doesn't support checking for specific versions.

	if (IsWindowsServer())
		windowsVersion += " Server";
	return windowsVersion;
#elif defined(__FreeBSD__)
	return std::string("FreeBSD | DankSpades: github.com/yusufcardinal/openspades");
#elif defined(__OpenBSD__)
	return std::string("OpenBSD | DankSpades: github.com/yusufcardinal/openspades");
#else
	return std::string("Unknown OS | DankSpades: github.com/yusufcardinal/openspades");
#endif
}
