# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: messages.proto

import sys
_b=sys.version_info[0]<3 and (lambda x:x) or (lambda x:x.encode('latin1'))
from google.protobuf.internal import enum_type_wrapper
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
from google.protobuf import descriptor_pb2
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='messages.proto',
  package='',
  serialized_pb=_b('\n\x0emessages.proto\"\x98\x01\n\x0bMmtpMessage\x12\x19\n\x07msgType\x18\x01 \x02(\x0e\x32\x08.MsgType\x12\x0c\n\x04uuid\x18\x02 \x02(\t\x12+\n\x0fprotocolMessage\x18\x04 \x01(\x0b\x32\x10.ProtocolMessageH\x00\x12+\n\x0fprsponseMessage\x18\x05 \x01(\x0b\x32\x10.ResponseMessageH\x00\x42\x06\n\x04\x62ody\"\xd2\x01\n\x0fResponseMessage\x12\x16\n\x0eresponseToUuid\x18\x01 \x02(\t\x12\x1f\n\x08response\x18\x02 \x02(\x0e\x32\r.ResponseEnum\x12\x12\n\nreasonText\x18\x03 \x01(\t\x12)\n\x0fmessageMetadata\x18\x04 \x03(\x0b\x32\x10.MessageMetadata\x12/\n\x12\x61pplicationMessage\x18\x05 \x03(\x0b\x32\x13.ApplicationMessage\x12\x16\n\x0ereconnectToken\x18\x06 \x01(\t\"J\n\x0fMessageMetadata\x12\x0c\n\x04uuid\x18\x01 \x02(\t\x12)\n\x06header\x18\x02 \x02(\x0b\x32\x19.ApplicationMessageHeader\"`\n\x12\x41pplicationMessage\x12)\n\x06header\x18\x01 \x02(\x0b\x32\x19.ApplicationMessageHeader\x12\x0c\n\x04\x62ody\x18\x02 \x02(\x0c\x12\x11\n\tsignature\x18\x03 \x02(\t\" \n\nRecipients\x12\x12\n\nrecipients\x18\x01 \x03(\t\"\xb5\x01\n\x18\x41pplicationMessageHeader\x12\x11\n\x07subject\x18\x01 \x01(\tH\x00\x12!\n\nrecipients\x18\x02 \x01(\x0b\x32\x0b.RecipientsH\x00\x12\x0f\n\x07\x65xpires\x18\x03 \x02(\x03\x12\x0e\n\x06sender\x18\x04 \x02(\t\x12\x12\n\nqosProfile\x18\x05 \x01(\t\x12\x18\n\x10\x62odySizeNumBytes\x18\x06 \x02(\rB\x14\n\x12SubjectOrRecipient\"\xcc\x02\n\x0fProtocolMessage\x12-\n\x0fprotocolMsgType\x18\x01 \x02(\x0e\x32\x14.ProtocolMessageType\x12&\n\x10subscribeMessage\x18\x02 \x01(\x0b\x32\n.SubscribeH\x00\x12*\n\x12unsubscribeMessage\x18\x03 \x01(\x0b\x32\x0c.UnsubscribeH\x00\x12\x1c\n\x0bsendMessage\x18\x04 \x01(\x0b\x32\x05.SendH\x00\x12\"\n\x0ereceiveMessage\x18\x05 \x01(\x0b\x32\x08.ReceiveH\x00\x12\x1e\n\x0c\x66\x65tchMessage\x18\x06 \x01(\x0b\x32\x06.FetchH\x00\x12(\n\x11\x64isconnectMessage\x18\x07 \x01(\x0b\x32\x0b.DisconnectH\x00\x12\"\n\x0e\x63onnectMessage\x18\x08 \x01(\x0b\x32\x08.ConnectH\x00\x42\x06\n\x04\x62ody\"S\n\tSubscribe\x12\x11\n\x07subject\x18\x01 \x01(\tH\x00\x12\x18\n\x0e\x64irectMessages\x18\x02 \x01(\x08H\x00\x42\x19\n\x17subjectOrDirectMessages\"U\n\x0bUnsubscribe\x12\x11\n\x07subject\x18\x01 \x01(\tH\x00\x12\x18\n\x0e\x64irectMessages\x18\x02 \x01(\x08H\x00\x42\x19\n\x17subjectOrDirectMessages\"7\n\x04Send\x12/\n\x12\x61pplicationMessage\x18\x01 \x02(\x0b\x32\x13.ApplicationMessage\"\"\n\x07Receive\x12\x17\n\x06\x66ilter\x18\x01 \x01(\x0b\x32\x07.Filter\"\x1e\n\x06\x46ilter\x12\x14\n\x0cmessageUuids\x18\x01 \x03(\t\"\x07\n\x05\x46\x65tch\"\x0c\n\nDisconnect\"1\n\x07\x43onnect\x12\x0e\n\x06ownMrn\x18\x01 \x01(\t\x12\x16\n\x0ereconnectToken\x18\x02 \x01(\t*N\n\x07MsgType\x12\x17\n\x13UNSPECIFIED_MESSAGE\x10\x00\x12\x14\n\x10PROTOCOL_MESSAGE\x10\x01\x12\x14\n\x10REDPONSE_MESSAGE\x10\x02*\xbc\x01\n\x13ProtocolMessageType\x12\x0f\n\x0bUNSPECIFIED\x10\x00\x12\x15\n\x11SUBSCRIBE_MESSAGE\x10\x01\x12\x16\n\x12UNSUBCRIBE_MESSAGE\x10\x02\x12\x10\n\x0cSEND_MESSAGE\x10\x03\x12\x13\n\x0fRECIEVE_MESSAGE\x10\x04\x12\x11\n\rFETCH_MESSAGE\x10\x05\x12\x16\n\x12\x44ISCONNECT_MESSAGE\x10\x06\x12\x13\n\x0f\x43ONNECT_MESSAGE\x10\x07*=\n\x0cResponseEnum\x12\x18\n\x14UNSPECIFIED_RESPONSE\x10\x00\x12\x08\n\x04GOOD\x10\x01\x12\t\n\x05\x45RROR\x10\x02')
)
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

_MSGTYPE = _descriptor.EnumDescriptor(
  name='MsgType',
  full_name='MsgType',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='UNSPECIFIED_MESSAGE', index=0, number=0,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='PROTOCOL_MESSAGE', index=1, number=1,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='REDPONSE_MESSAGE', index=2, number=2,
      options=None,
      type=None),
  ],
  containing_type=None,
  options=None,
  serialized_start=1484,
  serialized_end=1562,
)
_sym_db.RegisterEnumDescriptor(_MSGTYPE)

MsgType = enum_type_wrapper.EnumTypeWrapper(_MSGTYPE)
_PROTOCOLMESSAGETYPE = _descriptor.EnumDescriptor(
  name='ProtocolMessageType',
  full_name='ProtocolMessageType',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='UNSPECIFIED', index=0, number=0,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='SUBSCRIBE_MESSAGE', index=1, number=1,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='UNSUBCRIBE_MESSAGE', index=2, number=2,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='SEND_MESSAGE', index=3, number=3,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='RECIEVE_MESSAGE', index=4, number=4,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='FETCH_MESSAGE', index=5, number=5,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='DISCONNECT_MESSAGE', index=6, number=6,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='CONNECT_MESSAGE', index=7, number=7,
      options=None,
      type=None),
  ],
  containing_type=None,
  options=None,
  serialized_start=1565,
  serialized_end=1753,
)
_sym_db.RegisterEnumDescriptor(_PROTOCOLMESSAGETYPE)

ProtocolMessageType = enum_type_wrapper.EnumTypeWrapper(_PROTOCOLMESSAGETYPE)
_RESPONSEENUM = _descriptor.EnumDescriptor(
  name='ResponseEnum',
  full_name='ResponseEnum',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='UNSPECIFIED_RESPONSE', index=0, number=0,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='GOOD', index=1, number=1,
      options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='ERROR', index=2, number=2,
      options=None,
      type=None),
  ],
  containing_type=None,
  options=None,
  serialized_start=1755,
  serialized_end=1816,
)
_sym_db.RegisterEnumDescriptor(_RESPONSEENUM)

ResponseEnum = enum_type_wrapper.EnumTypeWrapper(_RESPONSEENUM)
UNSPECIFIED_MESSAGE = 0
PROTOCOL_MESSAGE = 1
REDPONSE_MESSAGE = 2
UNSPECIFIED = 0
SUBSCRIBE_MESSAGE = 1
UNSUBCRIBE_MESSAGE = 2
SEND_MESSAGE = 3
RECIEVE_MESSAGE = 4
FETCH_MESSAGE = 5
DISCONNECT_MESSAGE = 6
CONNECT_MESSAGE = 7
UNSPECIFIED_RESPONSE = 0
GOOD = 1
ERROR = 2



_MMTPMESSAGE = _descriptor.Descriptor(
  name='MmtpMessage',
  full_name='MmtpMessage',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='msgType', full_name='MmtpMessage.msgType', index=0,
      number=1, type=14, cpp_type=8, label=2,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='uuid', full_name='MmtpMessage.uuid', index=1,
      number=2, type=9, cpp_type=9, label=2,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='protocolMessage', full_name='MmtpMessage.protocolMessage', index=2,
      number=4, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='prsponseMessage', full_name='MmtpMessage.prsponseMessage', index=3,
      number=5, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
    _descriptor.OneofDescriptor(
      name='body', full_name='MmtpMessage.body',
      index=0, containing_type=None, fields=[]),
  ],
  serialized_start=19,
  serialized_end=171,
)


_RESPONSEMESSAGE = _descriptor.Descriptor(
  name='ResponseMessage',
  full_name='ResponseMessage',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='responseToUuid', full_name='ResponseMessage.responseToUuid', index=0,
      number=1, type=9, cpp_type=9, label=2,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='response', full_name='ResponseMessage.response', index=1,
      number=2, type=14, cpp_type=8, label=2,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='reasonText', full_name='ResponseMessage.reasonText', index=2,
      number=3, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='messageMetadata', full_name='ResponseMessage.messageMetadata', index=3,
      number=4, type=11, cpp_type=10, label=3,
      has_default_value=False, default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='applicationMessage', full_name='ResponseMessage.applicationMessage', index=4,
      number=5, type=11, cpp_type=10, label=3,
      has_default_value=False, default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='reconnectToken', full_name='ResponseMessage.reconnectToken', index=5,
      number=6, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=174,
  serialized_end=384,
)


_MESSAGEMETADATA = _descriptor.Descriptor(
  name='MessageMetadata',
  full_name='MessageMetadata',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='uuid', full_name='MessageMetadata.uuid', index=0,
      number=1, type=9, cpp_type=9, label=2,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='header', full_name='MessageMetadata.header', index=1,
      number=2, type=11, cpp_type=10, label=2,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=386,
  serialized_end=460,
)


_APPLICATIONMESSAGE = _descriptor.Descriptor(
  name='ApplicationMessage',
  full_name='ApplicationMessage',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='header', full_name='ApplicationMessage.header', index=0,
      number=1, type=11, cpp_type=10, label=2,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='body', full_name='ApplicationMessage.body', index=1,
      number=2, type=12, cpp_type=9, label=2,
      has_default_value=False, default_value=_b(""),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='signature', full_name='ApplicationMessage.signature', index=2,
      number=3, type=9, cpp_type=9, label=2,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=462,
  serialized_end=558,
)


_RECIPIENTS = _descriptor.Descriptor(
  name='Recipients',
  full_name='Recipients',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='recipients', full_name='Recipients.recipients', index=0,
      number=1, type=9, cpp_type=9, label=3,
      has_default_value=False, default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=560,
  serialized_end=592,
)


_APPLICATIONMESSAGEHEADER = _descriptor.Descriptor(
  name='ApplicationMessageHeader',
  full_name='ApplicationMessageHeader',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='subject', full_name='ApplicationMessageHeader.subject', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='recipients', full_name='ApplicationMessageHeader.recipients', index=1,
      number=2, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='expires', full_name='ApplicationMessageHeader.expires', index=2,
      number=3, type=3, cpp_type=2, label=2,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='sender', full_name='ApplicationMessageHeader.sender', index=3,
      number=4, type=9, cpp_type=9, label=2,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='qosProfile', full_name='ApplicationMessageHeader.qosProfile', index=4,
      number=5, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='bodySizeNumBytes', full_name='ApplicationMessageHeader.bodySizeNumBytes', index=5,
      number=6, type=13, cpp_type=3, label=2,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
    _descriptor.OneofDescriptor(
      name='SubjectOrRecipient', full_name='ApplicationMessageHeader.SubjectOrRecipient',
      index=0, containing_type=None, fields=[]),
  ],
  serialized_start=595,
  serialized_end=776,
)


_PROTOCOLMESSAGE = _descriptor.Descriptor(
  name='ProtocolMessage',
  full_name='ProtocolMessage',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='protocolMsgType', full_name='ProtocolMessage.protocolMsgType', index=0,
      number=1, type=14, cpp_type=8, label=2,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='subscribeMessage', full_name='ProtocolMessage.subscribeMessage', index=1,
      number=2, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='unsubscribeMessage', full_name='ProtocolMessage.unsubscribeMessage', index=2,
      number=3, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='sendMessage', full_name='ProtocolMessage.sendMessage', index=3,
      number=4, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='receiveMessage', full_name='ProtocolMessage.receiveMessage', index=4,
      number=5, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='fetchMessage', full_name='ProtocolMessage.fetchMessage', index=5,
      number=6, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='disconnectMessage', full_name='ProtocolMessage.disconnectMessage', index=6,
      number=7, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='connectMessage', full_name='ProtocolMessage.connectMessage', index=7,
      number=8, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
    _descriptor.OneofDescriptor(
      name='body', full_name='ProtocolMessage.body',
      index=0, containing_type=None, fields=[]),
  ],
  serialized_start=779,
  serialized_end=1111,
)


_SUBSCRIBE = _descriptor.Descriptor(
  name='Subscribe',
  full_name='Subscribe',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='subject', full_name='Subscribe.subject', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='directMessages', full_name='Subscribe.directMessages', index=1,
      number=2, type=8, cpp_type=7, label=1,
      has_default_value=False, default_value=False,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
    _descriptor.OneofDescriptor(
      name='subjectOrDirectMessages', full_name='Subscribe.subjectOrDirectMessages',
      index=0, containing_type=None, fields=[]),
  ],
  serialized_start=1113,
  serialized_end=1196,
)


_UNSUBSCRIBE = _descriptor.Descriptor(
  name='Unsubscribe',
  full_name='Unsubscribe',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='subject', full_name='Unsubscribe.subject', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='directMessages', full_name='Unsubscribe.directMessages', index=1,
      number=2, type=8, cpp_type=7, label=1,
      has_default_value=False, default_value=False,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
    _descriptor.OneofDescriptor(
      name='subjectOrDirectMessages', full_name='Unsubscribe.subjectOrDirectMessages',
      index=0, containing_type=None, fields=[]),
  ],
  serialized_start=1198,
  serialized_end=1283,
)


_SEND = _descriptor.Descriptor(
  name='Send',
  full_name='Send',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='applicationMessage', full_name='Send.applicationMessage', index=0,
      number=1, type=11, cpp_type=10, label=2,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=1285,
  serialized_end=1340,
)


_RECEIVE = _descriptor.Descriptor(
  name='Receive',
  full_name='Receive',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='filter', full_name='Receive.filter', index=0,
      number=1, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=1342,
  serialized_end=1376,
)


_FILTER = _descriptor.Descriptor(
  name='Filter',
  full_name='Filter',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='messageUuids', full_name='Filter.messageUuids', index=0,
      number=1, type=9, cpp_type=9, label=3,
      has_default_value=False, default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=1378,
  serialized_end=1408,
)


_FETCH = _descriptor.Descriptor(
  name='Fetch',
  full_name='Fetch',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=1410,
  serialized_end=1417,
)


_DISCONNECT = _descriptor.Descriptor(
  name='Disconnect',
  full_name='Disconnect',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=1419,
  serialized_end=1431,
)


_CONNECT = _descriptor.Descriptor(
  name='Connect',
  full_name='Connect',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='ownMrn', full_name='Connect.ownMrn', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='reconnectToken', full_name='Connect.reconnectToken', index=1,
      number=2, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=1433,
  serialized_end=1482,
)

_MMTPMESSAGE.fields_by_name['msgType'].enum_type = _MSGTYPE
_MMTPMESSAGE.fields_by_name['protocolMessage'].message_type = _PROTOCOLMESSAGE
_MMTPMESSAGE.fields_by_name['prsponseMessage'].message_type = _RESPONSEMESSAGE
_MMTPMESSAGE.oneofs_by_name['body'].fields.append(
  _MMTPMESSAGE.fields_by_name['protocolMessage'])
_MMTPMESSAGE.fields_by_name['protocolMessage'].containing_oneof = _MMTPMESSAGE.oneofs_by_name['body']
_MMTPMESSAGE.oneofs_by_name['body'].fields.append(
  _MMTPMESSAGE.fields_by_name['prsponseMessage'])
_MMTPMESSAGE.fields_by_name['prsponseMessage'].containing_oneof = _MMTPMESSAGE.oneofs_by_name['body']
_RESPONSEMESSAGE.fields_by_name['response'].enum_type = _RESPONSEENUM
_RESPONSEMESSAGE.fields_by_name['messageMetadata'].message_type = _MESSAGEMETADATA
_RESPONSEMESSAGE.fields_by_name['applicationMessage'].message_type = _APPLICATIONMESSAGE
_MESSAGEMETADATA.fields_by_name['header'].message_type = _APPLICATIONMESSAGEHEADER
_APPLICATIONMESSAGE.fields_by_name['header'].message_type = _APPLICATIONMESSAGEHEADER
_APPLICATIONMESSAGEHEADER.fields_by_name['recipients'].message_type = _RECIPIENTS
_APPLICATIONMESSAGEHEADER.oneofs_by_name['SubjectOrRecipient'].fields.append(
  _APPLICATIONMESSAGEHEADER.fields_by_name['subject'])
_APPLICATIONMESSAGEHEADER.fields_by_name['subject'].containing_oneof = _APPLICATIONMESSAGEHEADER.oneofs_by_name['SubjectOrRecipient']
_APPLICATIONMESSAGEHEADER.oneofs_by_name['SubjectOrRecipient'].fields.append(
  _APPLICATIONMESSAGEHEADER.fields_by_name['recipients'])
_APPLICATIONMESSAGEHEADER.fields_by_name['recipients'].containing_oneof = _APPLICATIONMESSAGEHEADER.oneofs_by_name['SubjectOrRecipient']
_PROTOCOLMESSAGE.fields_by_name['protocolMsgType'].enum_type = _PROTOCOLMESSAGETYPE
_PROTOCOLMESSAGE.fields_by_name['subscribeMessage'].message_type = _SUBSCRIBE
_PROTOCOLMESSAGE.fields_by_name['unsubscribeMessage'].message_type = _UNSUBSCRIBE
_PROTOCOLMESSAGE.fields_by_name['sendMessage'].message_type = _SEND
_PROTOCOLMESSAGE.fields_by_name['receiveMessage'].message_type = _RECEIVE
_PROTOCOLMESSAGE.fields_by_name['fetchMessage'].message_type = _FETCH
_PROTOCOLMESSAGE.fields_by_name['disconnectMessage'].message_type = _DISCONNECT
_PROTOCOLMESSAGE.fields_by_name['connectMessage'].message_type = _CONNECT
_PROTOCOLMESSAGE.oneofs_by_name['body'].fields.append(
  _PROTOCOLMESSAGE.fields_by_name['subscribeMessage'])
_PROTOCOLMESSAGE.fields_by_name['subscribeMessage'].containing_oneof = _PROTOCOLMESSAGE.oneofs_by_name['body']
_PROTOCOLMESSAGE.oneofs_by_name['body'].fields.append(
  _PROTOCOLMESSAGE.fields_by_name['unsubscribeMessage'])
_PROTOCOLMESSAGE.fields_by_name['unsubscribeMessage'].containing_oneof = _PROTOCOLMESSAGE.oneofs_by_name['body']
_PROTOCOLMESSAGE.oneofs_by_name['body'].fields.append(
  _PROTOCOLMESSAGE.fields_by_name['sendMessage'])
_PROTOCOLMESSAGE.fields_by_name['sendMessage'].containing_oneof = _PROTOCOLMESSAGE.oneofs_by_name['body']
_PROTOCOLMESSAGE.oneofs_by_name['body'].fields.append(
  _PROTOCOLMESSAGE.fields_by_name['receiveMessage'])
_PROTOCOLMESSAGE.fields_by_name['receiveMessage'].containing_oneof = _PROTOCOLMESSAGE.oneofs_by_name['body']
_PROTOCOLMESSAGE.oneofs_by_name['body'].fields.append(
  _PROTOCOLMESSAGE.fields_by_name['fetchMessage'])
_PROTOCOLMESSAGE.fields_by_name['fetchMessage'].containing_oneof = _PROTOCOLMESSAGE.oneofs_by_name['body']
_PROTOCOLMESSAGE.oneofs_by_name['body'].fields.append(
  _PROTOCOLMESSAGE.fields_by_name['disconnectMessage'])
_PROTOCOLMESSAGE.fields_by_name['disconnectMessage'].containing_oneof = _PROTOCOLMESSAGE.oneofs_by_name['body']
_PROTOCOLMESSAGE.oneofs_by_name['body'].fields.append(
  _PROTOCOLMESSAGE.fields_by_name['connectMessage'])
_PROTOCOLMESSAGE.fields_by_name['connectMessage'].containing_oneof = _PROTOCOLMESSAGE.oneofs_by_name['body']
_SUBSCRIBE.oneofs_by_name['subjectOrDirectMessages'].fields.append(
  _SUBSCRIBE.fields_by_name['subject'])
_SUBSCRIBE.fields_by_name['subject'].containing_oneof = _SUBSCRIBE.oneofs_by_name['subjectOrDirectMessages']
_SUBSCRIBE.oneofs_by_name['subjectOrDirectMessages'].fields.append(
  _SUBSCRIBE.fields_by_name['directMessages'])
_SUBSCRIBE.fields_by_name['directMessages'].containing_oneof = _SUBSCRIBE.oneofs_by_name['subjectOrDirectMessages']
_UNSUBSCRIBE.oneofs_by_name['subjectOrDirectMessages'].fields.append(
  _UNSUBSCRIBE.fields_by_name['subject'])
_UNSUBSCRIBE.fields_by_name['subject'].containing_oneof = _UNSUBSCRIBE.oneofs_by_name['subjectOrDirectMessages']
_UNSUBSCRIBE.oneofs_by_name['subjectOrDirectMessages'].fields.append(
  _UNSUBSCRIBE.fields_by_name['directMessages'])
_UNSUBSCRIBE.fields_by_name['directMessages'].containing_oneof = _UNSUBSCRIBE.oneofs_by_name['subjectOrDirectMessages']
_SEND.fields_by_name['applicationMessage'].message_type = _APPLICATIONMESSAGE
_RECEIVE.fields_by_name['filter'].message_type = _FILTER
DESCRIPTOR.message_types_by_name['MmtpMessage'] = _MMTPMESSAGE
DESCRIPTOR.message_types_by_name['ResponseMessage'] = _RESPONSEMESSAGE
DESCRIPTOR.message_types_by_name['MessageMetadata'] = _MESSAGEMETADATA
DESCRIPTOR.message_types_by_name['ApplicationMessage'] = _APPLICATIONMESSAGE
DESCRIPTOR.message_types_by_name['Recipients'] = _RECIPIENTS
DESCRIPTOR.message_types_by_name['ApplicationMessageHeader'] = _APPLICATIONMESSAGEHEADER
DESCRIPTOR.message_types_by_name['ProtocolMessage'] = _PROTOCOLMESSAGE
DESCRIPTOR.message_types_by_name['Subscribe'] = _SUBSCRIBE
DESCRIPTOR.message_types_by_name['Unsubscribe'] = _UNSUBSCRIBE
DESCRIPTOR.message_types_by_name['Send'] = _SEND
DESCRIPTOR.message_types_by_name['Receive'] = _RECEIVE
DESCRIPTOR.message_types_by_name['Filter'] = _FILTER
DESCRIPTOR.message_types_by_name['Fetch'] = _FETCH
DESCRIPTOR.message_types_by_name['Disconnect'] = _DISCONNECT
DESCRIPTOR.message_types_by_name['Connect'] = _CONNECT
DESCRIPTOR.enum_types_by_name['MsgType'] = _MSGTYPE
DESCRIPTOR.enum_types_by_name['ProtocolMessageType'] = _PROTOCOLMESSAGETYPE
DESCRIPTOR.enum_types_by_name['ResponseEnum'] = _RESPONSEENUM

MmtpMessage = _reflection.GeneratedProtocolMessageType('MmtpMessage', (_message.Message,), dict(
  DESCRIPTOR = _MMTPMESSAGE,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:MmtpMessage)
  ))
_sym_db.RegisterMessage(MmtpMessage)

ResponseMessage = _reflection.GeneratedProtocolMessageType('ResponseMessage', (_message.Message,), dict(
  DESCRIPTOR = _RESPONSEMESSAGE,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:ResponseMessage)
  ))
_sym_db.RegisterMessage(ResponseMessage)

MessageMetadata = _reflection.GeneratedProtocolMessageType('MessageMetadata', (_message.Message,), dict(
  DESCRIPTOR = _MESSAGEMETADATA,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:MessageMetadata)
  ))
_sym_db.RegisterMessage(MessageMetadata)

ApplicationMessage = _reflection.GeneratedProtocolMessageType('ApplicationMessage', (_message.Message,), dict(
  DESCRIPTOR = _APPLICATIONMESSAGE,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:ApplicationMessage)
  ))
_sym_db.RegisterMessage(ApplicationMessage)

Recipients = _reflection.GeneratedProtocolMessageType('Recipients', (_message.Message,), dict(
  DESCRIPTOR = _RECIPIENTS,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:Recipients)
  ))
_sym_db.RegisterMessage(Recipients)

ApplicationMessageHeader = _reflection.GeneratedProtocolMessageType('ApplicationMessageHeader', (_message.Message,), dict(
  DESCRIPTOR = _APPLICATIONMESSAGEHEADER,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:ApplicationMessageHeader)
  ))
_sym_db.RegisterMessage(ApplicationMessageHeader)

ProtocolMessage = _reflection.GeneratedProtocolMessageType('ProtocolMessage', (_message.Message,), dict(
  DESCRIPTOR = _PROTOCOLMESSAGE,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:ProtocolMessage)
  ))
_sym_db.RegisterMessage(ProtocolMessage)

Subscribe = _reflection.GeneratedProtocolMessageType('Subscribe', (_message.Message,), dict(
  DESCRIPTOR = _SUBSCRIBE,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:Subscribe)
  ))
_sym_db.RegisterMessage(Subscribe)

Unsubscribe = _reflection.GeneratedProtocolMessageType('Unsubscribe', (_message.Message,), dict(
  DESCRIPTOR = _UNSUBSCRIBE,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:Unsubscribe)
  ))
_sym_db.RegisterMessage(Unsubscribe)

Send = _reflection.GeneratedProtocolMessageType('Send', (_message.Message,), dict(
  DESCRIPTOR = _SEND,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:Send)
  ))
_sym_db.RegisterMessage(Send)

Receive = _reflection.GeneratedProtocolMessageType('Receive', (_message.Message,), dict(
  DESCRIPTOR = _RECEIVE,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:Receive)
  ))
_sym_db.RegisterMessage(Receive)

Filter = _reflection.GeneratedProtocolMessageType('Filter', (_message.Message,), dict(
  DESCRIPTOR = _FILTER,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:Filter)
  ))
_sym_db.RegisterMessage(Filter)

Fetch = _reflection.GeneratedProtocolMessageType('Fetch', (_message.Message,), dict(
  DESCRIPTOR = _FETCH,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:Fetch)
  ))
_sym_db.RegisterMessage(Fetch)

Disconnect = _reflection.GeneratedProtocolMessageType('Disconnect', (_message.Message,), dict(
  DESCRIPTOR = _DISCONNECT,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:Disconnect)
  ))
_sym_db.RegisterMessage(Disconnect)

Connect = _reflection.GeneratedProtocolMessageType('Connect', (_message.Message,), dict(
  DESCRIPTOR = _CONNECT,
  __module__ = 'messages_pb2'
  # @@protoc_insertion_point(class_scope:Connect)
  ))
_sym_db.RegisterMessage(Connect)


# @@protoc_insertion_point(module_scope)