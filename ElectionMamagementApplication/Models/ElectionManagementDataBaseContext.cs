using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ElectionMamagementApplication.Models;

public partial class ElectionManagementDataBaseContext : DbContext
{
    public ElectionManagementDataBaseContext()
    {
    }

    public ElectionManagementDataBaseContext(DbContextOptions<ElectionManagementDataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<Constituency> Constituencies { get; set; }

    public virtual DbSet<Election> Elections { get; set; }

    public virtual DbSet<ElectionsResult> ElectionsResults { get; set; }

    public virtual DbSet<Party> Parties { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    public virtual DbSet<Voter> Voters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MsSqlLocalDb;Initial Catalog=ElectionManagementDataBase;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Candidat__3214EC0784334A98");

            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Constituency).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.ConstituencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Candidates_Constituencies");

            entity.HasOne(d => d.Party).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.PartyId)
                .HasConstraintName("FK_Candidates_Parties");

            entity.HasOne(d => d.Election).WithMany(p => p.Candidates)
              .HasForeignKey(d => d.ElectionId)
              .HasConstraintName("FK_Candidates_Elections");
        });

        modelBuilder.Entity<Constituency>(entity =>
        {
            entity.HasKey(e => e.ConstituencyId).HasName("PK__Constitu__AD6DB4AF87263205");

            entity.Property(e => e.ConstituencyName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Election>(entity =>
        {
            entity.HasKey(e => e.ElectionId).HasName("PK__Election__C626C4263AA1C735");

            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ElectionName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ElectionStatus)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.EndDateTime).HasColumnType("datetime");
            entity.Property(e => e.StartDateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<ElectionsResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__Election__9769020835925C20");

         //   entity.Property(e => e.ResultId).ValueGeneratedNever();
            entity.Property(e => e.PercentageVotes)
                .HasDefaultValueSql("((0.00))")
                .HasColumnType("decimal(16, 2)");

            entity.HasOne(d => d.Candidate).WithMany(p => p.ElectionsResults)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ElectionsResults_Candidates");

            entity.HasOne(d => d.Election).WithMany(p => p.ElectionsResults)
                .HasForeignKey(d => d.ElectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ElectionsResults_Elections");
        });

        modelBuilder.Entity<Party>(entity =>
        {
            entity.HasKey(e => e.PartyId).HasName("PK__Parties__1640CD339C8369DF");

            entity.Property(e => e.FoundedYear).HasColumnType("date");
            entity.Property(e => e.LeaderName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PartyDscription)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PartyName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.VoteId).HasName("PK__Votes__52F015C28D5D9807");

            //entity.Property(e => e.VoteId).ValueGeneratedNever();
            entity.Property(e => e.VoteTimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Candidate).WithMany(p => p.Votes)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Votes_Candidates");

            entity.HasOne(d => d.Election).WithMany(p => p.Votes)
                .HasForeignKey(d => d.ElectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Votes_Elections");

            entity.HasOne(d => d.Voter).WithMany(p => p.Votes)
                .HasForeignKey(d => d.VoterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Votes_Voters");
        });

        modelBuilder.Entity<Voter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Voters__3214EC07FE4607B2");

            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Constituency).WithMany(p => p.Voters)
                .HasForeignKey(d => d.ConstituencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voters_Constituencies");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
