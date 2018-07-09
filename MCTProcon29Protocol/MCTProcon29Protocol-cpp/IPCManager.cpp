#include "stdafx.h"
#include "IIPCReaderAction.h"
#include "IPCManager.h"


void IPCManager::readMethod(void)
{
	while (true) {
		
	}
}

void IPCManager::write(char * data, int szData)
{
	DWORD dwResult;
	if (szData <= sizeof(buffer))
		WriteFile(hPipe, data, szData, &dwResult, NULL);
	else {
		int count = szData / sizeof(buffer);
		int end = szData / sizeof(buffer);
		for (int i = 0; i < count; ++i)
			WriteFile(hPipe, data + (i * sizeof(buffer)), sizeof(buffer), &dwResult, NULL);
		if (end > 0)
			WriteFile(hPipe, data + (count * sizeof(buffer)), end, &dwResult, NULL);
	}
}

IPCManager::IPCManager()
{
}


IPCManager::~IPCManager()
{
}

void IPCManager::startServer(IIPCReaderAction * act)
{
	action = act;

	OVERLAPPED oOverlapped;
	auto hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

	if (hEvent == INVALID_HANDLE_VALUE)
		throw std::system_error(std::error_code(), "Failed to Create Event.");

	oOverlapped.hEvent = hEvent;

	hPipe = CreateNamedPipe(
		L"\\\\.\\pipe\\MCTProcon29",
		PIPE_ACCESS_DUPLEX,
		PIPE_TYPE_BYTE | PIPE_WAIT,
		1,
		sizeof(buffer),
		sizeof(buffer),
		0,
		NULL);
	if (hPipe == INVALID_HANDLE_VALUE)
		throw std::system_error(std::error_code(), "Failed to Open NamedPipe.");

	if (ConnectNamedPipe(hPipe, &oOverlapped) == 0)
		throw std::system_error(std::error_code(), "Failed to Connect NamedPipe.");

	readThread = std::thread(&readMethod);
	isRunning = true;
}

void IPCManager::endServer(void)
{
	isRunning = false;
}
