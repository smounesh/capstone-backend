namespace JobPortal.Enums
{
    public enum UserRole
    {
        Admin,
        Applicant,
        Employer
    }
    public enum DeletedBy
    {
        Admin,
        User
    }
    public enum JobType
    {
        FullTime,
        PartTime,
        Internship,
        Contract
    }

    public enum SeniorityLevel
    {
        Entry,
        Junior,
        Intermediate,
        Senior,
        Executive
    }

    public enum JobStatus
    {
        Open,
        Closed,
        Filled
    }

    public enum ResumeStatus
    {
        Active,      
        Inactive,    
        Archived      
    }
    public enum JobApplicationStatus
    {
        Submitted,
        Reviewed,
        Interviewed,
        Rejected,
        Accepted,
        OnHold,
        Withdrawn
    }
}
