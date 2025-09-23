namespace ShiemiApi.Services
{
	public class UserStorageService
	{
		private Dict<string,string> Connections = new ();

		public string GetConnectionId(string userId)
			=> Connections.GetValue(userId);

		public void SetConnectionId(string userId, string connectionId)
			=> Connections.Add(userId, connectionId);

		public void RemoveConnectionId(string userId)
			=> Connections.Remove(userId);
	}
}
