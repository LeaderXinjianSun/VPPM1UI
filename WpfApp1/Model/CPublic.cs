using System;
using System.Collections.Generic;
using System.Text;
//using System.Windows.Forms;

namespace 读写器530SDK
{
    class CPublic
    {
        static int m_languageFlag = 1;
        //去掉字符中非16进制的字符
        static public string StrToHexStr(string str)            
        {
            string strTmp = "";
	        int i,len;
            len = str.Length;
            for (i = 0; i < len; i++)
            {
                if ((str[i] >= 'a' && str[i] <= 'f') || (str[i] >= 'A' && str[i] <= 'F') || (str[i] >= '0' && str[i] <= '9') || str[i] == 0)
                {
                    strTmp += (char)str[i];//可以这样？？
                }
            }
            return strTmp;//返回一个临时的变量，有问题啊
        }
        //把16进制的字符串数字转换为int型数据
        public static int HexStringToInt(string hexStr)
        {
            string strTmp = StrDelete0X(hexStr);
            int iOutput = 0;

            try
            {
                iOutput = int.Parse(strTmp, System.Globalization.NumberStyles.HexNumber);
            }
            catch (FormatException)
            {
                //MessageBox.Show("Please enter hex number!", "warning", MessageBoxButtons.OK);
                return 1;//返回值 需要进行修改和考虑
            }
            
            return iOutput;
        }
        //去除16进制数的前缀"0x"或者"0X"
        public static string StrDelete0X(string str)
        {
            string strTmp = "";
            //byte[] byteTmp = new byte[16];
            if (str.Length > 1)
            {
                if (('x' == str[1] || 'X' == str[1]) && ('0' == str[0]))
                {
                    strTmp += (char)str[2];
                    strTmp += (char)str[3];
                }
                else
                {
                    strTmp = str;
                }
            }
            else if (str.Length == 1)
            {
                strTmp = str;
            }

            return strTmp;
        }
        //把一个字符串，去掉非16进制字符，两两组合，转换为整数，放在一个数组中
        public static byte[] CharToByte(string str)
        {
            string strTmp;
            strTmp = StrToHexStr(str);
            str = strTmp;
            strTmp = "";
            byte []tmpByte = new byte[1024];
	        for(int i = 0;i < str.Length;i += 2)
	        {
                strTmp += str[i];
                if (i + 1 < str.Length)
                {
                    strTmp += str[i + 1];
                }
		        tmpByte[i / 2] = (byte)HexStringToInt(strTmp);
                strTmp = "";
	        }
            return tmpByte;
        }

        public static string ApiError(int apiReturn)
        {
            string str = "";
            if(m_languageFlag == 0)
            {
                switch(apiReturn)
                {
                case 0x00:
                    str = "Successfully\r\n";
                    break;
                case 0x01:
                    str = "error\r\n";
	                break;
                case 0x02:
                    str = "unmatched length of receiving data\r\n";
	                break;
                case 0x03:
                    str = "failure sending of COM\r\n";
                    break;
                case 0x04:
                    str = "nothing receiving from COM\r\n";
                    break;
                case 0x05:
                    str = "unmatched address of equipment\r\n";
                    break;
                case 0x07:
                    str = "failure checksum\r\n";
                    break;
                case 0x0A:
                    str = "failure input parameters\r\n";
                    break;
                default:
                    str = "not define error\r\n";
                	break;
                }
            }
            else
            {
                switch(apiReturn)
                {
                case 0x00:
                    str = "命令执行成功\r\n";
                    break;
                case 0x01:
                    str = "命令操作失败\r\n";
	                break;
                case 0x02:
                    str = "接收数据长度不匹配\r\n";
	                break;
                case 0x03:
                    str = "串口发送失败\r\n";
                    break;
                case 0x04:
                    str = "串口未接到任何数据\r\n";
                    break;
                case 0x05:
                    str = "设备地址不匹配\r\n";
                    break;
                case 0x07:
                    str = "校验和不正确\r\n";
                    break;
                case 0x0A:
                    str = "输入参数有误，请参见不具体的函数说明\r\n";
                    break;
                default:
                    str = "未定义的操作错误\r\n";
                	break;
                }
            }
            return str;
        }

        public static string ReturnCodeError(ref byte buf)
        {
            string str = "";
            if (m_languageFlag == 0)
            {
                switch (buf)
                {
                    case 0x00:
                        str = "successfully\r\n";
                        break;
                    case 0x01:
                        str = "error\r\n";
                        break;
                    case 0x80:
                        str = "setting successfully\r\n";
                        break;
                    case 0x81:
                        str = "Fail operating\r\n";
                        break;
                    case 0x82:
                        str = "error,overtime\r\n";
                        break;
                    case 0x83:
                        str = "error,no card\r\n";
                        break;
                    case 0x84:
                        str = "error,data of card\r\n";
                        break;
                    case 0x85:
                        str = "error,Incorrect input parameter or command format\r\n";
                        break;
                    case 0x87:
                        str = "unknown failure\r\n";
                        break;
                    case 0x89:
                        str = "error,Incorrect input parameter or command format\r\n";
                        break;
                    case 0x8A:
                        str = "Initial block error\r\n";
                        break;
                    case 0x8B:
                        str = "wrong card Serial Number in the anti-collision\r\n";
                        break;
                    case 0x8C:
                        str = "password Authentication failed\r\n";
                        break;
                    case 0x8f:
                        str = "error,Input command code not exist\r\n";
                        break;
                    case 0x90:
                        str = "The card were unmatched for the command\r\n";
                        break;
                    case 0x91:
                        str = "error in order format\r\n";
                        break;
                    case 0x92:
                        str = "unmatched FLAG parameter and OPTION parameter\r\n";
                        break;
                    case 0x93:
                        str = "inexistent block\r\n";
                        break;
                    case 0x94:
                        str = "locked,unchangeable operation\r\n";
                        break;
                    case 0x95:
                        str = "locking operation unsuccessfully\r\n";
                        break;
                    case 0x96:
                        str = "write operation unsuccessfully\r\n";
                        break;
                    default:
                        str = "not define error\r\n";
                        break;
                }
            }
            else
            {
                switch (buf)
                {
                    case 0x00:
                        str = "命令执行成功\r\n";
                        break;
                    case 0x01:
                        str = "命令操作失败（具体说明参见函数）\r\n";
                        break;
                    case 0x80:
                        str = "参数设置成功\r\n";
                        break;
                    case 0x81:
                        str = "参数设置失败\r\n";
                        break;
                    case 0x82:
                        str = "通讯超时\r\n";
                        break;
                    case 0x83:
                        str = "卡不存在\r\n";
                        break;
                    case 0x84:
                        str = "表示接收卡数据出错\r\n";
                        break;
                    case 0x85:
                        str = "输入参数或者输入命令格式错误\r\n";
                        break;
                    case 0x87:
                        str = "未知的错误\r\n";
                        break;
                    case 0x89:
                        str = "输入参数或者输入命令格式错误\r\n";
                        break;
                    case 0x8A:
                        str = "在块初始化中出现错误\r\n";
                        break;
                    case 0x8B:
                        str = "防冲突过程中得到错误的序列号\r\n";
                        break;
                    case 0x8C:
                        str = "密码认证没有通过\r\n";
                        break;
                    case 0x8f:
                        str = "输入的指令代码不存在\r\n";
                        break;
                    case 0x90:
                        str = "表示卡不支持这个命令\r\n";
                        break;
                    case 0x91:
                        str = "命令格式有错误\r\n";
                        break;
                    case 0x92:
                        str = "在命令的FLAG参数中，不支持OPTION 模式\r\n";
                        break;
                    case 0x93:
                        str = "要操作的BLOCK不存在\r\n";
                        break;
                    case 0x94:
                        str = "要操作的对象已经别锁定，不能进行修改\r\n";
                        break;
                    case 0x95:
                        str = "锁定操作不成功\r\n";
                        break;
                    case 0x96:
                        str = "写操作不成功\r\n";
                        break;
                    default:
                        str = "未定义的操作错误\r\n";
                        break;
                }
            }
            return str;
        }
    }
}
