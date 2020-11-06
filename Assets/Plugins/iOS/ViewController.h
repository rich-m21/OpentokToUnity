#import <UIKit/UIKit.h>

@interface ViewController : UIViewController
@property (nonatomic) NSString *kSessionId;
@property (nonatomic) NSString *kToken;
@property (nonatomic) NSString *kType;
@property (nonatomic) NSString *patient;
@property (nonatomic) BOOL audioMute;
@property (nonatomic) BOOL videoMute;
@property (nonatomic) BOOL callback1;
@property (nonatomic) BOOL callback2;
@property (nonatomic) BOOL hideViewCallback1;
@property (nonatomic) BOOL hideViewCallback2;
@property (nonatomic) BOOL showViewCallback1;
@property (nonatomic) BOOL showViewCallback2;
@property (nonatomic) UIButton *muteButton;
@property (nonatomic) UIButton *hangUpButton;
@property (nonatomic) UIButton *videoButton;
@property (nonatomic) UIButton *historyButton;
-(void)startSession;
-(void)closeSession;
-(void) DisconnectSession;
-(void)MuteAudioToggleAction:(id)sender;
-(void)MuteVideoToggleAction:(id)sender;
-(void)HideView;
-(void)ShowView;
@end
