using CollectApp.Models;
using CollectApp.ViewModels;
using Humanizer;

namespace CollectAppTests.Builders;

public class ChangeCollectViewModelBuilder
{
    private int _id = 1;
    private CollectStatus _status = CollectStatus.PendenteAprovar;
    private bool _toOpen = false;
    private string _userId = "user123";
    private bool _canChangeCollectStatus = true;
    private bool _canEditOpenOrDeleteCollect = true;

    public ChangeCollectViewModelBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ChangeCollectViewModelBuilder WithCollectStatus(CollectStatus status)
    {
        _status = status;
        return this;
    }

    public ChangeCollectViewModelBuilder WithToOpen(bool toOpen)
    {
        _toOpen = toOpen;
        return this;
    }

    public ChangeCollectViewModelBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public ChangeCollectViewModelBuilder WithCanChangeCollectStatus(bool canChangeCollectStatus)
    {
        _canChangeCollectStatus = canChangeCollectStatus;
        return this;
    }

    public ChangeCollectViewModelBuilder WithCanEditOpenOrDeleteCollect(bool canEditOpenOrDeleteCollect)
    {
        _canEditOpenOrDeleteCollect = canEditOpenOrDeleteCollect;
        return this;
    }

    public ChangeCollectViewModel Build()
    {
        return new ChangeCollectViewModel
        {
            Id = _id,
            Status = _status,
            ToOpen = _toOpen,
            UserId = _userId,
            CanChangeCollectStatus = _canChangeCollectStatus,
            CanEditOpenOrDeleteCollect = _canEditOpenOrDeleteCollect,
        };      
    }
};