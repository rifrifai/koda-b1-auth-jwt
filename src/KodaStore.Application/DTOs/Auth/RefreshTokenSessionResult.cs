namespace KodaStore.Application.DTOs.Auth;

public record class RefreshTokenSessionResult
(
	AuthUserResponse User,
	IssuedAccessToken AccessToken,
	string RawRefreshToken,
	DateTimeOffset RefreshExpiredAtUtc
);
