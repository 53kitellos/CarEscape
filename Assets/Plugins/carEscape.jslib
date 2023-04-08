mergeInto(LibraryManager.library, 
{
  SetPlayerData: function ()
  {
    myGameInstance.SendMessage('ForYandex', 'SetName',player.getName());
  },

  SaveInfoExtern: function (date)
  { 
    var dateString = UTF8ToString(date);
    var myobj = JSON.parse(dateString);
    player.setData(myobj);
    console.log(myobj);
  },

  LoadInfoExtern: function ()
  {
    player.getData().then(_date =>{
      const myJSON = JSON.stringify(_date);
      myGameInstance.SendMessage('Progress', 'LoadPlayerInfo', myJSON);
      console.log(myJSON);
    });
  },

  SetInLeaderbord : function(value,levelIndex){
    ysdk.getLeaderboards()
    .then(lb => {
      console.log(value); 
      if (levelIndex == 1) {console.log("SOHRANENIE11111111111"); lb.setLeaderboardScore('Level1Time', value);}
      if (levelIndex == 2) {console.log("SOHRANENIE22222222222"); lb.setLeaderboardScore('Level2Time', value);}
      if (levelIndex == 3) {console.log("SOHRANENIE33333333333"); lb.setLeaderboardScore('Level3Time', value);}
      if (levelIndex == 4) {lb.setLeaderboardScore('Level4Time', value);}
      if (levelIndex == 5) {lb.setLeaderboardScore('Level5Time', value);}
      if (levelIndex == 6) {lb.setLeaderboardScore('Level6Time', value);}
      if (levelIndex == 7) {lb.setLeaderboardScore('Level7Time', value);}
      if (levelIndex == 8) {lb.setLeaderboardScore('Level8Time', value);}
      if (levelIndex == 9) {lb.setLeaderboardScore('Level9Time', value);}
    });
  },

  GetLang: function () {
    var lang = ysdk.environment.i18n.lang;
    var bufferSize =  (lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  InitUserDevice: function (){
    var device = ysdk.deviceInfo.type;
    var bufferSize = lengthBytesUTF8(device) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(device, buffer, bufferSize);
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