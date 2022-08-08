using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses;

namespace AdLerBackend.Application.Moodle.Commands.GetUserData;

public record GetMoodleUserDataCommand : CommandWithToken<MoodleUserDataResponse>;