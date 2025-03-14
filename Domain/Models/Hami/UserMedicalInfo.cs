﻿using Domain.Models.Common;
using Domain.Models.IdentityAggregate;
using Domain.Primitives;
using FluentResults;
using SharedKernel.Errors;
using Domain.Models.Hami.Events;
using System.ComponentModel;

namespace Domain.Models.Hami;

public class UserMedicalInfo : Entity
{
    private UserMedicalInfo(Guid id) : base(id) { }

    private UserMedicalInfo() : base(Guid.NewGuid()) { }

    // متد ایجاد (Factory Method)
    public static UserMedicalInfo Create(
        string userId,
        Organ organ,
        DiseaseType diseaseType,
        PatientStatus patientStatus,
        int? stage,
        string? pathologyDiagnosis,
        float? initialWeight,
        int? sleepDuration,
        AppetiteLevel appetiteLevel)
    {
        return new UserMedicalInfo(Guid.NewGuid())
        {
            UserId = userId,
            Organ = organ,
            DiseaseType = diseaseType,
            PatientStatus = patientStatus,
            Stage = stage,
            PathologyDiagnosis = pathologyDiagnosis,
            InitialWeight = initialWeight,
            SleepDuration = sleepDuration,
            AppetiteLevel = appetiteLevel
        };
    }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; } = null;
    public Organ Organ { get; set; } // ارگان درگیر
    public DiseaseType DiseaseType { get; set; } // نوع بیماری
    public PatientStatus PatientStatus { get; set; } // وضعیت بیمار
    public int? Stage { get; set; } // مرحله بیماری (1،2،3،4)
    public string? PathologyDiagnosis { get; set; } // تشخیص پاتولوژی (متن اختیاری)
    public float? InitialWeight { get; set; } // وزن اولیه کاربر
    public int? SleepDuration { get; set; } // مدت خوابیدن (ساعت در شبانه‌روز)
    public AppetiteLevel AppetiteLevel { get; set; } // میزان اشتها

    public ICollection<MedicalEntry> MedicalEntries { get; set; } = new List<MedicalEntry>();


    public static UserMedicalInfo Register(
       string userId,
        Organ organ,
        DiseaseType diseaseType,
        PatientStatus patientStatus,
        int? stage,
        string? pathologyDiagnosis,
        float? initialWeight,
        int? sleepDuration,
        AppetiteLevel appetiteLevel,
        string phoneNumber)
    {
        UserMedicalInfo info = new(Guid.NewGuid());
        info.UserId = userId;
        info.Organ = organ;
        info.DiseaseType = diseaseType;
        info.PatientStatus = patientStatus;
        info.Stage = stage;
        info.PathologyDiagnosis = pathologyDiagnosis;
        info.InitialWeight = initialWeight;
        info.SleepDuration = sleepDuration;
        info.AppetiteLevel = appetiteLevel;

        //if(info.User is null)
            //return UserErrors.UserNotExsists;
        info.Raise(new PatientCreatedDomainEvent(
            info.Id,
            info.UserId,
            phoneNumber));
        return info;
    }


}

public enum Organ
{
    [Description("تخمدان")]
    Ovary = 1,

    [Description("پستان")]
    Breast = 2,

    [Description("پروستات")]
    Prostate = 3,

    [Description("مراقب ")]
    Caregiver = 4,
}

public enum DiseaseType
{
    [Description("خوش‌خیم")]
    Benign = 1,

    [Description("بدخیم")]
    Malignant = 2,
}

public enum PatientStatus
{
    [Description("تازه تشخیص")]
    NewlyDiagnosed = 1,

    [Description("تحت درمان")]
    UnderTreatment = 2,

    [Description("تکمیل درمان")]
    TreatmentCompleted = 3,

    [Description("عود بیماری")]
    Relapsed = 4,
}

public enum AppetiteLevel
{
    [Description("زیاد")]
    High = 1,

    [Description("معمولی")]
    Normal = 2,

    [Description("بی‌اشتها")]
    Low = 3,
}