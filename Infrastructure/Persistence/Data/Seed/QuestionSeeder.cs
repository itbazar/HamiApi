using Domain.Models.Hami;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data.Seed;

public static class QuestionSeeder
{
    public static void SeedQuestions(ModelBuilder modelBuilder)
    {
        // سوالات GAD با GUID ثابت
        var gadQuestions = new List<Question>
        {
            Question.Create(TestType.GAD, "داشتن احساس بی قراری، خشم، اضطراب، عصبانیت", new Guid("ecc379cf-0ef3-4498-a865-6c320369f66a")),
            Question.Create(TestType.GAD, "ناتوانی در توقف یا کنترل نگرانی", new Guid("d4debcde-5a59-4f3f-b568-b8375bc3a19f")),
            Question.Create(TestType.GAD, "نگرانی بیش از حد پیرامون مسائل مختلف", new Guid("d061d84d-6dce-4d27-81fb-2f100a18a375")),
            Question.Create(TestType.GAD, "اشکال در آرامش داشتن (عدم توانایی در حفظ آرامش خود)", new Guid("fbe74744-276c-4470-8aec-45488b1ae921")),
            Question.Create(TestType.GAD, "بی قراری شدید به حدی که نمی توانم بنشینم", new Guid("34b900ca-53f3-4322-b06c-300e54f0cd95")),
            Question.Create(TestType.GAD, "به سهولت عصبی یا بی قرار می شوم", new Guid("1ea79543-e61a-4d7c-a4c5-d879da4d1f71")),
            Question.Create(TestType.GAD, "ترس این رو دارم که هر لحظه اتفاق بدی بیوفتد", new Guid("9b991b96-19e1-4f30-a699-56a977a20b34"))
        };

        // سوالات MDD با GUID ثابت
        var mddQuestions = new List<Question>
        {
            Question.Create(TestType.MDD, "علاقه یا لذت کم در اجرای کار ها (علاقه یا لذت کمی برای انجام دادن کار ها دارم)", new Guid("5b32abcf-3506-4ebc-959b-f2a941511ef5")),
            Question.Create(TestType.MDD, "احساس افسردگی، مود پایین یا نا امیدی دارم", new Guid("60c8f10d-0496-4254-b8c1-99ff0b662d8f")),
            Question.Create(TestType.MDD, "اختلال خواب (به سختی خواب میروم، در خواب بیدار می شوم و یا خیلی زیاد می خوابم)", new Guid("249a84d8-64ad-4a41-b68f-7d0f175e9b92")),
            Question.Create(TestType.MDD, "احساس خستگی، پایین بودن انرژی دارم", new Guid("3a55fbe0-b8f6-4f41-bca1-66e565b78442")),
            Question.Create(TestType.MDD, "اختلال در اشتها (اشتهایم کم شده ویا زیاد غذا می خورم)", new Guid("32d9f476-7bbc-4136-b899-4962982cca5c")),
            Question.Create(TestType.MDD, "احساس بدی نسبت به خود دارم، احساس شکست میکنم، احساس میکنم خودم یا خانواده ام را ناامید کرده ام", new Guid("69a6b0fc-07a6-48e7-be89-62dac9bc198a")),
            Question.Create(TestType.MDD, "تمرکز در انجام کارها ندارم مثلا زمانی که مطالعه میکنم یا تلویزیون میبینم", new Guid("59b47353-e8b0-4f1f-a0da-53bdbc4ed8a0")),
            Question.Create(TestType.MDD, "حرکات یا صحبت کردنم به قدری آهسته است که دیگران متوجه آن می شوند یا برعکس آنقدر بی قرارم که خیلی بیشتر از حد معمول در حرکتم", new Guid("5f76a2a9-eb8a-40af-ac55-d60d31bb82d7")),
            Question.Create(TestType.MDD, "افکاری در مورد مردن یا آسیب زدن به خود به سراغم می آید", new Guid("8dda7f6e-cc89-4638-8f48-2453b9240132"))
        };

        // ادغام سوالات GAD و MDD
        var allQuestions = gadQuestions.Concat(mddQuestions).ToList();

        // اضافه کردن داده‌ها به ModelBuilder
        foreach (var question in allQuestions)
        {
            modelBuilder.Entity<Question>().HasData(new
            {
                Id = question.Id,
                TestType = question.TestType,
                QuestionText = question.QuestionText,
                IsDeleted = question.IsDeleted
            });
        }
    }
}
