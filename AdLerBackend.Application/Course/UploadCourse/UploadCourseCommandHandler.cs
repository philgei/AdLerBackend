﻿using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.Commands.GetUserData;
using MediatR;

namespace AdLerBackend.Application.Course.UploadCourse;

public class UploadCourseCommandHandler : IRequestHandler<UploadCourseCommand, bool>
{
    private readonly ILmsBackupProcessor _lmsBackupProcessor;
    private readonly IMediator _mediator;

    public UploadCourseCommandHandler(ILmsBackupProcessor lmsBackupProcessor, IMediator mediator)
    {
        _lmsBackupProcessor = lmsBackupProcessor;
        _mediator = mediator;
    }

    public async Task<bool> Handle(UploadCourseCommand request, CancellationToken cancellationToken)
    {
        var userInformation = await GetUserInformation(request);
        if (!userInformation.isAdmin) throw new ForbiddenAccessException("You are not an admin");

        var courseInformation = _lmsBackupProcessor.GetLevelDescriptionFromBackup(request.DslFileStream);

        IList<H5PDto> h5PFilesInBackup;
        if (courseInformation.LearningWorld.LearningElements.Any(x => x.ElementType == "h5p"))
            h5PFilesInBackup = _lmsBackupProcessor.GetH5PFilesFromBackup(request.H5PFileSteam);


        throw new NotImplementedException("Gagag");
    }

    private async Task<MoodleUserDataResponse> GetUserInformation(UploadCourseCommand request)
    {
        var userData = await _mediator.Send(new GetMoodleUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        });

        return userData;
    }
}