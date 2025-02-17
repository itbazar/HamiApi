using FluentResults;

namespace SharedKernel.Errors;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCaptcha = new Error("کپچا نادرست است.");
    public static readonly Error InvalidCredentials = new Error("اطلاعات ورودی اشتباه است.");
    public static readonly Error InvalidOtp = new Error("کد یکبار مصرف اشتباه است.");
    public static readonly Error InvalidUsername = new Error("نام کاربری نامعتبر است.");
    public static readonly Error InvalidPhoneNumber = new Error("شماره همراه نامعتبر است.");
    public static readonly Error InvalidRefereshToken = new Error("توکن نوسازی نامعتبر است.");
    public static readonly Error InvalidAccessToken = new Error("توکن دسترسی نامعتبر است.");
    public static readonly Error TokenNotExpiredYet = new Error("توکن دسترسی هنوز منقضی نشده است.");
    public static readonly Error UserNotFound = new Error("کاربر یافت نشد.");
    public static readonly Error UserCreationFailed = new Error("ایجاد کاربر با خطا مواجه شد.");
    public static readonly Error TooManyRequestsForOtp = new Error("درخواست رمز یکبارمصرف بیشتر از حد مجاز است.");
    public static readonly Error UserAlreadyExists = new Error("کاربری دیگری با شماره همراه شما ثبت شده است");
    public static readonly Error RegistrationNotApproved = new Error("وضعیت ثبت‌نام شما هنوز تأیید نشده و در انتظار تایید است.");
    public static readonly Error RegistrationRejected = new Error("وضعیت ثبت‌نام شما رد شده است.");


}
public static class CommunicationErrors
{
    public static readonly Error SmsError = new Error("خطایی در ارسال پیامک رخ داد.");
}

public static class GenericErrors
{
    public static readonly Error NotFound = new Error("موردی یافت نشد.");
    public static readonly Error AttachmentFailed = new Error("ذخیره پیوست با خطا مواجه شد.");
}

public static class EncryptionErrors
{
    public static readonly Error KeyGenerationFailed = new Error("ایجاد کلید با خطا مواجه شد.");
    public static readonly Error InvalidKey = new Error("کلید نامعتبر است.");
    public static readonly Error WrongPassword = new Error("رمز عبور نادرست است.");
    public static readonly Error KeyNotFound = new Error("کلید یافت نشد.");
    public static readonly Error Failed = new Error("رمزنگاری با خطا مواجه شد.");
    public static readonly Error NullCipher = new Error("بازیابی اطلاعات با خطا مواجه شد.");
    public static readonly Error InvalidHash = new Error("رمز عبور نادرست است.");
}

public static class UserErrors
{
    public static readonly Error UnExpected = new Error("خطایی غیرمنتظری پیش آمد.");
    public static readonly Error UserNotExsists = new Error("کاربر یافت نشد.");
    public static readonly Error PasswordUpdateFailed = new Error("بروزرسانی رمز عبور با خطا مواجه شد.");
    public static readonly Error RoleUpdateFailed = new Error("بروزرسانی نقش با خطا مواجه شد.");
    public static readonly Error UserIsNotPending = new Error("وضعیت ثبت نام کاربر در انتظار تایید نیست");
    public static readonly Error UserGroupNotAssigned = new Error("لطفا گروه کاربر را مشخص نمایید");
}

public static class ComplaintErrors
{
    public static readonly Error InconsistentContent = new Error("ناسازگاری در اطلاعات");
    public static readonly Error InvalidOperation = new Error("عملیات نامعتبر است.");
    public static readonly Error NotFound = new Error("درخواست یافت نشد.");
    public static readonly Error PublicKeyNotFound = new Error("کلید یافت نشد.");
}

public static class TestPeriodErrors
{
    public static readonly Error NotFound = new Error("آزمون یافت نشد.");
    public static readonly Error MoodExist = new Error("مود امروز خود را ثبت کرده اید.");
    public static readonly Error OutOfRangeDate = new Error("اجازه ثبت آزمون را ندارید به دلیل زمان نامناسب");
    public static readonly Error TestAlreadySubmitted = new Error("آزمون را ثبت کرده اید.");
}

public static class PatientLabTestErrors
{
    public static readonly Error NotFound = new Error("نوع تست یافت نشد.");
}

public static class SessionErrors
{
    public static readonly Error NotFound = new Error("جلسه یافت نشد.");
    public static readonly Error ISConfirmed = new Error("برگزاری جلسه قبلا تایید شده است و امکان ثبت مجدد حضور غیاب وجود ندارد.");
}

public static class PublicKeyErrors
{
    public static readonly Error InUsedKeyCannotBeDeleted = new Error("کلید فعال نمی تواند حذف شود.");
    public static readonly Error DeletedKeyCannotSetAsActive = new Error("کلید حذف شده نمیتواند به عنوان کلید فعال انتخاب شود.");
    public static readonly Error ThisKeyIsActiveAlready = new Error("کلید انتخابی در حال حاضر فعال است.");
    public static readonly Error ChangeKeyProblem = new Error("تغییر کلید فعال با خطا مواجه شد.");
    public static readonly Error UnderOperation = new Error("سامانه در حال بروزرسانی است.");
}