using Data;
using Services;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ItemsAttachmentPoints _attachmentPoints;

        private void Start()
        {
            var service = new ItemsPoolService();
            var clothdata = GameController.Services.Get<DataService>().ItemsData;
            service.Initialize(_attachmentPoints, clothdata);

            GameController.Services.Get<SaveLoadService>().LoadProgress();
            GameController.Services.Register(service);
            
        }
    }
}