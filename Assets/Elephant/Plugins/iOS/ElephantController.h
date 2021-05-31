
#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface ElephantController : NSObject
@end

#ifdef __cplusplus
extern "C" {
#endif

const char* IDFA();
const char* getConsentStatus();
void TestFunction(const char * string);
void showIdfaConsent(int type, int delay, int position, const char * titleText,
                     const char * descriptionText,
                     const char * buttonText,
                     const char * termsText,
                     const char * policyText,
                     const char * termsUrl,
                     const char * policyUrl);
void ElephantPost(const char * url, const char * body, const char * gameID, const char * authToken, int tryCount);

const char * ElephantCopyString(const char * string)
{
   char * copy = (char*)malloc(strlen(string) + 1);
   strcpy(copy, string);
   return copy;
}

    
#ifdef __cplusplus
} // extern "C"
#endif


NS_ASSUME_NONNULL_END
