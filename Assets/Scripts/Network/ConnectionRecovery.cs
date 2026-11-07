
using System;

namespace ManchTak
{
    public enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,
        Reconnecting,
        Failed
    }

    [Serializable]
    public class ConnectionRecoveryData
    {
        public string PlayerId;
        public string MatchId;
        public string ReconnectToken;

        public long LastConnectionTime;
        public long LastHeartbeatTime;

        public int ReconnectAttempts;
        public ConnectionState State;

        public ConnectionRecoveryData()
        {
            State = ConnectionState.Disconnected;
            ReconnectAttempts = 0;
            LastConnectionTime = 0;
            LastHeartbeatTime = 0;
        }
    }

    public static class ConnectionRecovery
    {
        public const int MaxReconnectAttempts = 8;
        public const int ReconnectIntervalSeconds = 3;
        public const int ConnectionTimeoutSeconds = 15;

        public static void SetConnecting(
            ConnectionRecoveryData data)
        {
            if (data == null)
                return;

            data.State = ConnectionState.Connecting;
        }

        public static void SetConnected(
            ConnectionRecoveryData data)
        {
            if (data == null)
                return;

            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            data.State = ConnectionState.Connected;
            data.LastConnectionTime = now;
            data.LastHeartbeatTime = now;
            data.ReconnectAttempts = 0;
        }

        public static void SetDisconnected(
            ConnectionRecoveryData data)
        {
            if (data == null)
                return;

            data.State = ConnectionState.Disconnected;
        }

        public static bool CanReconnect(
            ConnectionRecoveryData data)
        {
            if (data == null)
                return false;

            if (string.IsNullOrWhiteSpace(data.PlayerId) ||
                string.IsNullOrWhiteSpace(data.MatchId) ||
                string.IsNullOrWhiteSpace(data.ReconnectToken))
                return false;

            return data.ReconnectAttempts < MaxReconnectAttempts;
        }

        public static bool TryBeginReconnect(
            ConnectionRecoveryData data)
        {
            if (!CanReconnect(data))
                return false;

            data.ReconnectAttempts++;
            data.State = ConnectionState.Reconnecting;

            return true;
        }

        public static void SetReconnectFailed(
            ConnectionRecoveryData data)
        {
            if (data == null)
                return;

            if (data.ReconnectAttempts >= MaxReconnectAttempts)
                data.State = ConnectionState.Failed;
            else
                data.State = ConnectionState.Disconnected;
        }

        public static void UpdateHeartbeat(
            ConnectionRecoveryData data)
        {
            if (data == null ||
                data.State != ConnectionState.Connected)
                return;

            data.LastHeartbeatTime =
                DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public static bool IsConnectionTimedOut(
            ConnectionRecoveryData data)
        {
            if (data == null ||
                data.State != ConnectionState.Connected)
                return false;

            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return now - data.LastHeartbeatTime >
                   ConnectionTimeoutSeconds;
        }

        public static bool HasActiveMatch(
            ConnectionRecoveryData data)
        {
            return data != null &&
                   !string.IsNullOrWhiteSpace(data.MatchId);
        }

        public static void ClearMatch(
            ConnectionRecoveryData data)
        {
            if (data == null)
                return;

            data.MatchId = string.Empty;
            data.ReconnectToken = string.Empty;
            data.ReconnectAttempts = 0;
            data.State = ConnectionState.Disconnected;
        }
    }
}
