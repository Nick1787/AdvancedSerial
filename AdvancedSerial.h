/*
 *        File: AdvSerialInterface.h
 *      Author: Nick Dodds <Nick1787@gmail.com>
 * Description: An Adnvancer Serial Interface for Interfacing with Arduino
 * ----------------------------------------------------------------
 *    Revision:
 *    02262015 - NRD - Initial Version
 * ----------------------------------------------------------------
 */


#ifndef __ADVANCEDSERIAL_H__
#define __ADVANCEDSERIAL_H__

#include "LinkedList.h"
#include <Arduino.h>

//MESAGE DECODER
//
//  All Messages are composed of the following structure
//    |--Header------------|-DATA--------------------|-EOT--|
//    #ASI:<MSGKEY>:<MSGID>:..........................<CRNL>
//  
//    DATA:       TYPE:            DESCRIPTION:
//    <MSGKEY>     byte             Message KEY, A unique key for the type of message being sent
//    <MSGID>      uint16           Message ID,  A unique message ID is which is echo's back to transmitter to indicate a response to a message.
//    <DATA>       (varying)        Message Data, varying data types and length depending on message.
//    <CRNL>       char           Carriage Return + New Line Character signifying the end of a transmission.
//    <SymbolID>   uint             Symbol ID number
//    <SymbolName> String0          Symbol Name - Null Terminated String
//    <DTYPE>      byte             DataType  0=Boolean, 1=Byte, 2=short, 3=int, 4=unsigned int, 5=long, 6=unsigned long, 7=float, 8=double
//
//
//  -INCOMING-MESSAGES-
//    MSGKEY:   DATA#:    DATA:                           DESCRIPTION:
//     A0        0         --                              Request for available Symbols and Types
//     A1        0         --                              Request for Symbol data as a single message (all symbols).
//
//  -OUTGOING-MESSAGES-
//    MSGKEY:   DATA#:    DATA:                           DESCRIPTION:
//     B0        N         <SymbolID><DTYPE><SymbolName>   Up to N Items. Response to request for available symbols.
//     B1        N         <SymbolID><DATA>                Up to N Items. Response to request for symbol data.
//  

#ifndef _advancedserial_inbuffer
  #define _advancedserial_inbuffer 200
#endif

typedef enum dataType{ asi_bool, asi_byte, asi_short, asi_long, asi_ushort, asi_ulong, asi_int, asi_uint, asi_float, asi_double};
struct LoggedSymbol{
  String Name;
  dataType Type;
  void * addr;
};

class AdvancedSerial{
  
//variables
public:
protected:
private:
  LinkedList<LoggedSymbol> symbols = LinkedList<LoggedSymbol>();
  HardwareSerial *SerialRef;
  int ReadBuffPos = 0;
  
//functions
public:
  void addSymbol(String Name, bool * value);
  void addSymbol(String Name, float * value);
  void addSymbol(String Name, double * value);
  void addSymbol(String Name, unsigned long * value);
  void addSymbol(String Name, int * value);
  
  void exec();
  AdvancedSerial(HardwareSerial *SerialRef);
  ~AdvancedSerial();
  
  void TransmitSymbolList(unsigned int MessageID);
  void TransmitSymbolData(unsigned int MessageID);
protected:
private:
  void Read();
  void ProcessInMsg(char * buffer);

  union {
    bool val;
    byte bval[1];
  } boolCvt;

  union {
    short val;
    byte bval[2];
  } shortCvt;
  
  union {
    int val;
    byte bval[2];
  } intCvt;
  
  union {
    unsigned int val;
    byte bval[2];
  } uintCvt;
  
  union {
    long val;
    byte bval[4];
  } lngCvt;
  
  union {
    unsigned long val;
    byte bval[4];
  } ulngCvt;
  
  union {
    float val;
    byte bval[4];
  } fltCvt;
  
  union {
    double val;
    byte bval[8];
  } dblCvt;
  
}; //AdvancedSerial

#endif //__ADVANCEDSERIAL_H__

