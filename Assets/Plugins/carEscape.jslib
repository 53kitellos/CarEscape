mergeInto(LibraryManager.library, 
{

  SetPlayerData: function ()
  {
    myGameInstance.SendMessage('ForYandex', 'SetName',player.getName());
    myGameInstance.SendMessage('ForYandex', 'SetIcon',player.getPhoto("small")); 
  },

  SaveInfoExtern: function (date)
  {
    var dataString = UTF8ToString(date);
    var myobj = JSON.parse(dateString);
    player.setData(myobj);
    console.log(myobj);
  },

  LoadInfoExtern: function ()
  {
    player.getData().then(_date =>{
      const myJSON = JSON.stringify(_date);
      myGameInstance.SendMessage('Progress', 'LoadPlayerInfo',myJSON);
      console.log(myJSON);
    });
  },

  SetInLeaderbord : function(value){
    ysdk.getLeaderboards()
    .then(lb => {
    // Без extraData
      lb.setLeaderboardScore('BestPeopleTime', value);
    });
  },

  GetLang: function () {
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  ShowAdv : function(){
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          // some action after close
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

  ShowExternRewardAdv : function(){
    ysdk.adv.showRewardedVideo({
    callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          console.log('Rewarded!');
        },
        onClose: () => {
          console.log('Video ad closed.');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
    }
})
  },

  AskGameFeedback: function ()
  {
    ysdk.feedback.canReview()
    .then(({ value, reason }) => {
      if (value) {
        ysdk.feedback.requestReview()
        .then(({ feedbackSent }) => {
          myGameInstance.SendMessage('ForYandex','RateDone', player.feedbackSent);
          console.log(feedbackSent);
        })
      } else {
        console.log(reason)
      }
    })
  },
});