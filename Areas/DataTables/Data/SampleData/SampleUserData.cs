using MCS.HomeSite.Areas.Models.Users;

namespace MCS.HomeSite.Areas.DataTables.Data.SampleData;

public static class SampleUserData
{
    public static IEnumerable<UserDto> Data()
    {
        return new[]
        {
            new UserDto
            {
                Id = 1,
                Email = "test.user1@email.com",
                Name = "Test User 1",
                UserName = "TestUser1",
                Password = "securepassword1"
            },
            new UserDto
            {
                Id = 2,
                Email = "test.user2@email.com",
                Name = "Test User 2",
                UserName = "TestUser2",
                Password = "securepassword2"
            },
            new UserDto
            {
                Id = 3,
                Email = "test.user3@email.com",
                Name = "Test User 3",
                UserName = "TestUser3",
                Password = "securepassword3"
            },
            new UserDto
            {
                Id = 4,
                Email = "test.user4@email.com",
                Name = "Test User 4",
                UserName = "TestUser4",
                Password = "securepassword4"
            },
            new UserDto
            {
                Id = 5,
                Email = "test.user5@email.com",
                Name = "Test User 5",
                UserName = "TestUser5",
                Password = "securepassword5"
            },
            new UserDto
            {
                Id = 6,
                Email = "test.user6@email.com",
                Name = "Test User 6",
                UserName = "TestUser6",
                Password = "securepassword6"
            },
            new UserDto
            {
                Id = 7,
                Email = "test.user7@email.com",
                Name = "Test User 7",
                UserName = "TestUser7",
                Password = "securepassword7"
            },
            new UserDto
            {
                Id = 8,
                Email = "test.user8@email.com",
                Name = "Test User 8",
                UserName = "TestUser8",
                Password = "securepassword8"
            },
            new UserDto
            {
                Id = 9,
                Email = "test.user9@email.com",
                Name = "Test User 9",
                UserName = "TestUser9",
                Password = "securepassword9"
            },
            new UserDto
            {
                Id = 10,
                Email = "test.user10@email.com",
                Name = "Test User 10",
                UserName = "TestUser10",
                Password = "securepassword10"
            },
            new UserDto
            {
                Id = 11,
                Email = "test.user11@email.com",
                Name = "Test User 11",
                UserName = "TestUser11",
                Password = "securepassword11"
            },
            new UserDto
            {
                Id = 12,
                Email = "test.user12@email.com",
                Name = "Test User 12",
                UserName = "TestUser12",
                Password = "securepassword12"
            },
            new UserDto
            {
                Id = 13,
                Email = "test.user13@email.com",
                Name = "Test User 13",
                UserName = "TestUser13",
                Password = "securepassword13"
            },
            new UserDto
            {
                Id = 14,
                Email = "test.user14@email.com",
                Name = "Test User 14",
                UserName = "TestUser14",
                Password = "securepassword14"
            },
            new UserDto
            {
                Id = 15,
                Email = "test.user15@email.com",
                Name = "Test User 15",
                UserName = "TestUser15",
                Password = "securepassword15"
            }
        };
    }
}