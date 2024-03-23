// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: SocketGameProtocal.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SocketGameProtocal {

  /// <summary>Holder for reflection information generated from SocketGameProtocal.proto</summary>
  public static partial class SocketGameProtocalReflection {

    #region Descriptor
    /// <summary>File descriptor for SocketGameProtocal.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SocketGameProtocalReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChhTb2NrZXRHYW1lUHJvdG9jYWwucHJvdG8SElNvY2tldEdhbWVQcm90b2Nh",
            "bCLYAQoITWFpblBhY2sSNAoLcmVxdWVzdGNvZGUYASABKA4yHy5Tb2NrZXRH",
            "YW1lUHJvdG9jYWwuUmVxdWVzdENvZGUSMgoKYWN0aW9uY29kZRgCIAEoDjIe",
            "LlNvY2tldEdhbWVQcm90b2NhbC5BY3Rpb25Db2RlEjIKCnJldHVybmNvZGUY",
            "AyABKA4yHi5Tb2NrZXRHYW1lUHJvdG9jYWwuUmV0dXJuQ29kZRIuCghsaW5r",
            "cGFjaxgEIAEoCzIcLlNvY2tldEdhbWVQcm90b2NhbC5MaW5rUGFjayIcCghM",
            "aW5rUGFjaxIQCghQbGF5ZXJJRBgBIAEoBSooCgtSZXF1ZXN0Q29kZRIPCgtS",
            "ZXF1ZXN0Tm9uZRAAEggKBFVzZXIQASomCgpBY3Rpb25Db2RlEg4KCkFjdGlv",
            "bk5vbmUQABIICgRMaW5rEAEqNQoKUmV0dXJuQ29kZRIOCgpSZXR1cm5Ob25l",
            "EAASCwoHU3VjY2VlZBABEgoKBmZhaWxlZBACYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::SocketGameProtocal.RequestCode), typeof(global::SocketGameProtocal.ActionCode), typeof(global::SocketGameProtocal.ReturnCode), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SocketGameProtocal.MainPack), global::SocketGameProtocal.MainPack.Parser, new[]{ "Requestcode", "Actioncode", "Returncode", "Linkpack" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SocketGameProtocal.LinkPack), global::SocketGameProtocal.LinkPack.Parser, new[]{ "PlayerID" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  ///用来定位Controller
  /// </summary>
  public enum RequestCode {
    [pbr::OriginalName("RequestNone")] RequestNone = 0,
    [pbr::OriginalName("User")] User = 1,
  }

  /// <summary>
  ///用来定位方法
  /// </summary>
  public enum ActionCode {
    [pbr::OriginalName("ActionNone")] ActionNone = 0,
    /// <summary>
    ///连接
    /// </summary>
    [pbr::OriginalName("Link")] Link = 1,
  }

  public enum ReturnCode {
    [pbr::OriginalName("ReturnNone")] ReturnNone = 0,
    [pbr::OriginalName("Succeed")] Succeed = 1,
    [pbr::OriginalName("failed")] Failed = 2,
  }

  #endregion

  #region Messages
  /// <summary>
  ///主包
  /// </summary>
  public sealed partial class MainPack : pb::IMessage<MainPack> {
    private static readonly pb::MessageParser<MainPack> _parser = new pb::MessageParser<MainPack>(() => new MainPack());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MainPack> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SocketGameProtocal.SocketGameProtocalReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MainPack() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MainPack(MainPack other) : this() {
      requestcode_ = other.requestcode_;
      actioncode_ = other.actioncode_;
      returncode_ = other.returncode_;
      linkpack_ = other.linkpack_ != null ? other.linkpack_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MainPack Clone() {
      return new MainPack(this);
    }

    /// <summary>Field number for the "requestcode" field.</summary>
    public const int RequestcodeFieldNumber = 1;
    private global::SocketGameProtocal.RequestCode requestcode_ = global::SocketGameProtocal.RequestCode.RequestNone;
    /// <summary>
    ///Controller名
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SocketGameProtocal.RequestCode Requestcode {
      get { return requestcode_; }
      set {
        requestcode_ = value;
      }
    }

    /// <summary>Field number for the "actioncode" field.</summary>
    public const int ActioncodeFieldNumber = 2;
    private global::SocketGameProtocal.ActionCode actioncode_ = global::SocketGameProtocal.ActionCode.ActionNone;
    /// <summary>
    ///方法名
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SocketGameProtocal.ActionCode Actioncode {
      get { return actioncode_; }
      set {
        actioncode_ = value;
      }
    }

    /// <summary>Field number for the "returncode" field.</summary>
    public const int ReturncodeFieldNumber = 3;
    private global::SocketGameProtocal.ReturnCode returncode_ = global::SocketGameProtocal.ReturnCode.ReturnNone;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SocketGameProtocal.ReturnCode Returncode {
      get { return returncode_; }
      set {
        returncode_ = value;
      }
    }

    /// <summary>Field number for the "linkpack" field.</summary>
    public const int LinkpackFieldNumber = 4;
    private global::SocketGameProtocal.LinkPack linkpack_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SocketGameProtocal.LinkPack Linkpack {
      get { return linkpack_; }
      set {
        linkpack_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MainPack);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MainPack other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Requestcode != other.Requestcode) return false;
      if (Actioncode != other.Actioncode) return false;
      if (Returncode != other.Returncode) return false;
      if (!object.Equals(Linkpack, other.Linkpack)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Requestcode != global::SocketGameProtocal.RequestCode.RequestNone) hash ^= Requestcode.GetHashCode();
      if (Actioncode != global::SocketGameProtocal.ActionCode.ActionNone) hash ^= Actioncode.GetHashCode();
      if (Returncode != global::SocketGameProtocal.ReturnCode.ReturnNone) hash ^= Returncode.GetHashCode();
      if (linkpack_ != null) hash ^= Linkpack.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Requestcode != global::SocketGameProtocal.RequestCode.RequestNone) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Requestcode);
      }
      if (Actioncode != global::SocketGameProtocal.ActionCode.ActionNone) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Actioncode);
      }
      if (Returncode != global::SocketGameProtocal.ReturnCode.ReturnNone) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Returncode);
      }
      if (linkpack_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(Linkpack);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Requestcode != global::SocketGameProtocal.RequestCode.RequestNone) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Requestcode);
      }
      if (Actioncode != global::SocketGameProtocal.ActionCode.ActionNone) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Actioncode);
      }
      if (Returncode != global::SocketGameProtocal.ReturnCode.ReturnNone) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Returncode);
      }
      if (linkpack_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Linkpack);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MainPack other) {
      if (other == null) {
        return;
      }
      if (other.Requestcode != global::SocketGameProtocal.RequestCode.RequestNone) {
        Requestcode = other.Requestcode;
      }
      if (other.Actioncode != global::SocketGameProtocal.ActionCode.ActionNone) {
        Actioncode = other.Actioncode;
      }
      if (other.Returncode != global::SocketGameProtocal.ReturnCode.ReturnNone) {
        Returncode = other.Returncode;
      }
      if (other.linkpack_ != null) {
        if (linkpack_ == null) {
          Linkpack = new global::SocketGameProtocal.LinkPack();
        }
        Linkpack.MergeFrom(other.Linkpack);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Requestcode = (global::SocketGameProtocal.RequestCode) input.ReadEnum();
            break;
          }
          case 16: {
            Actioncode = (global::SocketGameProtocal.ActionCode) input.ReadEnum();
            break;
          }
          case 24: {
            Returncode = (global::SocketGameProtocal.ReturnCode) input.ReadEnum();
            break;
          }
          case 34: {
            if (linkpack_ == null) {
              Linkpack = new global::SocketGameProtocal.LinkPack();
            }
            input.ReadMessage(Linkpack);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///登录包
  /// </summary>
  public sealed partial class LinkPack : pb::IMessage<LinkPack> {
    private static readonly pb::MessageParser<LinkPack> _parser = new pb::MessageParser<LinkPack>(() => new LinkPack());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<LinkPack> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SocketGameProtocal.SocketGameProtocalReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LinkPack() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LinkPack(LinkPack other) : this() {
      playerID_ = other.playerID_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LinkPack Clone() {
      return new LinkPack(this);
    }

    /// <summary>Field number for the "PlayerID" field.</summary>
    public const int PlayerIDFieldNumber = 1;
    private int playerID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int PlayerID {
      get { return playerID_; }
      set {
        playerID_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as LinkPack);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(LinkPack other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (PlayerID != other.PlayerID) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (PlayerID != 0) hash ^= PlayerID.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (PlayerID != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(PlayerID);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (PlayerID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(PlayerID);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(LinkPack other) {
      if (other == null) {
        return;
      }
      if (other.PlayerID != 0) {
        PlayerID = other.PlayerID;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            PlayerID = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code