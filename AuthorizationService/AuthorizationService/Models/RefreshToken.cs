using System;

namespace AuthorizationService.Models
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiresDate { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        public RefreshToken() { }
       
        public RefreshToken(Guid accountId, DateTime createDate, int expiresSec)
        {
            AccountId = accountId;
            CreateDate = createDate;
            ExpiresDate = CreateDate.AddSeconds(expiresSec);
        }
    }
}
