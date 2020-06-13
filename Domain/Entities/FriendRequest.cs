namespace Domain.Entities
{
    class FriendRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PossibleFiendId { get; set; }
    }
}
