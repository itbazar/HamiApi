using FluentResults;

namespace SharedKernel.Successes;


public static class UpdateSuccess
{
    public static readonly Success Report = new Success("درخواست با موفقیت ویرایش شد.");
    public static readonly Success CategoryStatus = new Success("وضعیت دسته بندی به روز شد.");
    public static readonly Success Category = new Success(" دسته بندی به روز شد.");
    public static readonly Success Form = new Success(" فرم به روز شد.");
    public static readonly Success News = new Success(" خبر به روز شد.");
    public static readonly Success OrganizationalUnit = new Success(" واحد سازمانی به روز شد.");
    public static readonly Success Poll = new Success(" نظرسنجی به روز شد.");
    public static readonly Success Faq = new Success(" سوال متداول به روز شد.");
    public static readonly Success Process = new Success(" فرایند به روز شد.");
    public static readonly Success UserProfile = new Success(" پروفایل کاربر به روز شد.");
    public static readonly Success Avatar = new Success(" عکس پروفایل به روز شد.");
    public static readonly Success QuickAccess = new Success(" دسترسی سریع به روز شد.");
    public static readonly Success Password = new Success(" رمز عبور به روز شد.");
    public static readonly Success OperatorCategories = new Success(" دسته بندی های اپراتور به روز شد.");
    public static readonly Success Roles = new Success(" نقش ها به روز شد.");
    public static readonly Success Reagion = new Success(" مناطق به روز شد.");
    public static readonly Success PhoneNumber = new Success(" شماره موبایل به روز شد.");
    public static readonly Success ReportNote = new Success(" یادداشت به روز شد.");
}

public static class DeleteSuccess
{
    public static readonly Success Form = new Success(" فرم حذف شد.");
    public static readonly Success OrganizationalUnit = new Success(" واحد سازمانی حذف شد.");
    public static readonly Success Process = new Success(" فرایند حذف شد.");
    public static readonly Success Comment = new Success(" نظر حذف شد.");
    public static readonly Success ReportNote = new Success(" یادداشت حذف شد.");
}

public static class RegistrationSuccess
{
    public static readonly Success PollResponse = new Success("نظر شما با موفقیت ثبت شد.");
    public static readonly Success ReportNote = new Success("یادداشت شما با موفقیت ثبت شد.");
    public static readonly Success Feedback = new Success("بازخورد شما ثبت شد.");
}


public class OperationSuccess
{
    public static readonly Success MessageToCitizen = new Success("پیام به شهروند با موفقیت ارسال شد");
    public static readonly Success ResendOtp = new Success("کد تایید مجدد ارسال شد");
    public static readonly Success MakeTransition = new Success("درخواست با موفقیت ارجاع داده شد.");
    public static readonly Success MakeObjection = new Success("درخواست برای بازرسی ارجاع داده شد.");
    public static readonly Success ReplyComment = new Success("پاسخ شما با موفقیت ارسال شد.");
    public static readonly Success ViolationReview = new Success("گزارش تخلف بررسی شد.");
}
