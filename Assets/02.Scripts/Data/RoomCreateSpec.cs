public struct RoomCreateSpec
{
    public string Nickname;
    public string RoomName;

    public bool IsValid => !string.IsNullOrEmpty(Nickname) && !string.IsNullOrEmpty(RoomName);
}
