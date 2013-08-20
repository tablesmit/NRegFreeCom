#include "stdafx.h"

#pragma data_seg(".my_data")
int j = 42;                    

#pragma data_seg(".12345678")
int k = MAXINT;                    

#pragma data_seg(".my_guid")
GUID MYGUID = { 0x5dd75b4f, 0x6f63, 0x4bd7, { 0xa7, 0x56, 0xe9, 0x17, 0xfd, 0xf3, 0x84, 0x3 } };// {5DD75B4F-6F63-4BD7-A756-E917FDF38403}

#pragma data_seg(".my_str")
char* MYNAME = "My name is My name is My name is My name is My name is My name is My name is My name is My name is My name is My name is  My name is My name is My name is My name is My name is";

