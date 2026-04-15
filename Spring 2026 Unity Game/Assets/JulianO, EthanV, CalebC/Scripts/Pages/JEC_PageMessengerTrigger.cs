using System;

[Serializable]
public class JEC_PageMessengerTrigger
{
    public JEC_PageData page;
    public bool sendOnlyOnce = true;
    public JEC_MessengerMessage message;
}
