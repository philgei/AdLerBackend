using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Moodle.GetUserData;

public record GetMoodleUserDataCommand : CommandWithToken<MoodleUserDataResponse>;