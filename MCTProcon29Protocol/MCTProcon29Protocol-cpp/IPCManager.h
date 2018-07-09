#pragma once
class IPCManager
{
private:
	static const int BUFFER_SIZE = 1024;
	IIPCReaderAction* action = nullptr;
	bool isRunning = false;
	std::thread readThread;
	void readMethod(void);
	HANDLE hPipe;
	char buffer[BUFFER_SIZE];
	void write(char * data, int szData);

public:
	IPCManager();
	~IPCManager();
	void startServer(IIPCReaderAction* act);
	void endServer(void);
};

