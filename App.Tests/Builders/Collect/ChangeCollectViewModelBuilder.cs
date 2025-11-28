using CollectApp.Models;
using CollectApp.ViewModels;
using Humanizer;

namespace CollectAppTests.Builders;

public class ChangeCollectViewModelBuilder
{
    private int _id;
    private CollectStatus _status;
    private bool _toOpen = false;
    private string _userId = String.Empty;
    private bool _canChangeCollectStatus = true;
    private bool _canEditOpenOrDeleteCollect = true;

    public ChangeCollectViewModelBuilder FromCollect(Collect c)
    {
        _id = c.Id;
        _status = c.Status;
        _userId = c.UserId;
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