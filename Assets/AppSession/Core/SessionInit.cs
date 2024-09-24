using xPlugUniAdmissionManager.Assets.AppKits;

namespace xPlugUniAdmissionManager.Assets.AppSession.Core;

public class InitSession : IStartSession
{

    public void StartSession(ISession session)
    {
        var sessionId = Guid.NewGuid().ToString().ToCoreHash();
        session.SetString(StaticVals.APP_SESSION_ID, sessionId.ToString());

    }


}
