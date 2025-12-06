using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class ChangeCollectViewModelBuilder
{
    private int _id;
    private CollectStatus _status;
    private bool _toOpen = false;
    private string _userId = string.Empty;
    private bool _canChangeCollectStatus = true;
    private bool _canEditOpenOrDeleteCollect = true;

    public ChangeCollectViewModelBuilder FromCollect(Collect c)
    {
        _id = c.Id;
        _status = c.Status;
        _userId = c.UserId;
        return this;
    }

    public ChangeCollectViewModelBuilder WithToOpen(bool toOpen)
    {
        _toOpen = toOpen;
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