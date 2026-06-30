namespace AuthenticationService.Entities
{
    public partial class User
    {
        protected void SetUpdated(Guid updatedBy)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }
        public void UpdatePassword(string passwordHash, Guid updatedBy)
        {
            PasswordHash = passwordHash;
            SetUpdated(updatedBy);
        }

        public void VerifyEmail()
        {
            if (IsEmailVerified)
                return;

            IsEmailVerified = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
