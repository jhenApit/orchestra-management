using AutoMapper;
using OrchestraAPI.Dtos.Concert;
using OrchestraAPI.Dtos.Player;
using OrchestraAPI.Models;
using OrchestraAPI.Repositories.Players;
using OrchestraAPI.Repositories.Users;
using System.Reflection;

namespace OrchestraAPI.Services.Players
{
    public class PlayerService : IPlayerService
    {

        private readonly IPlayerRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public PlayerService(IPlayerRepository repository, IUserRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PlayerDto> AddPlayer(PlayerCreationDto player)
        {
            var user = await _userRepository.GetUserById(player.UserId);

            var newPlayer = new Player
            {
                Name = player.Name,
                UserId = player.UserId
            };

            newPlayer.Id = await _repository.AddPlayer(newPlayer);

            return _mapper.Map<PlayerDto>(newPlayer);
        }

        public async Task<bool> DeletePlayer(int id)
        {
            return await _repository.DeletePlayer(id);
        }

        public async Task<IEnumerable<PlayerDto>> GetAllPlayers()
        {
            var playerModel = await _repository.GetAllPlayers();
            return _mapper.Map<IEnumerable<PlayerDto>>(playerModel);
        }

        public async Task<PlayerDto?> GetPlayerById(int id)
        {
            var model = await _repository.GetPlayerById(id);
            if (model == null) return null;

            var dto = _mapper.Map<PlayerDto>(model);

            return dto;

        }

        public async Task<bool> UpdatePlayer(int id, PlayerUpdationDto playerToUpdate)
        {
            var player = await _repository.GetPlayerById(id);

            var playerModel = new Player
            {
                Name = playerToUpdate.Name
            };

            return await _repository.UpdatePlayer(id, playerModel);
        }

        public async Task<PlayerDto> UpdatePlayerScore(int id, PlayerScoreUpdateDto playerToUpdate)
        {
            var player = await _repository.GetPlayerById(id);
            player.Score = playerToUpdate.Score;

            var updatedPlayerScore = await _repository.UpdatePlayerScore(id, player);

            var dto = _mapper.Map<PlayerDto>(player);

            return dto;
        }

        public bool IsIdValid(int id)
        {
            return id > 0 && IsNumeric(id.ToString());
        }

        private bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }
    }
}
