using AutoMapper;
using ElectionMamagementApplication.Models;
using ElectionMamagementApplication.ModelView;

namespace ElectionMamagementApplication.Config
{
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Candidate, CandidateView>();
                CreateMap<Voter, VoterView>();
                CreateMap<Election, ElectionView>();
                CreateMap<Vote, VoteView>();
                CreateMap<ElectionsResult,ElectionResultView>();
                CreateMap<Constituency, ConstituencyView>();
                CreateMap<Party, PartyView>();

            CreateMap<CandidateView, Candidate>();
            CreateMap<VoterView, Voter>();
            CreateMap<ElectionView, Election>();
            CreateMap<VoteView, Vote>();
            CreateMap<ElectionResultView, ElectionsResult>();
            CreateMap<ConstituencyView, Constituency>();
            CreateMap<PartyView, Party>();

            CreateMap<ElectionResultViewOut, ElectionsResult>();
            CreateMap<ElectionsResult,ElectionResultViewOut>();
            CreateMap<VoterViewOut, Voter>();
            CreateMap<Voter,VoterViewOut>();
        }
    }
    }

