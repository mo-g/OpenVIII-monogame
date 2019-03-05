﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
#if _WINDOWS
using DirectMidi;
#endif
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using NAudio;
using NAudio.Wave;

namespace FF8
{
    static class init_debugger_Audio
    {
#if _WINDOWS
        private static CDirectMusic cdm;
        private static CDLSLoader loader;
        private static CSegment segment;
        private static CAPathPerformance path;
        public static CPortPerformance cport; //public explicit
        private static COutputPort outport;
        private static CCollection ccollection;
        private static CInstrument[] instruments;
#endif

        private struct SoundEntry
        {
            public int Size;
            public int Offset;
            public byte[] UNK; //12
            public byte[] WAVFORMATEX; //18
            public ushort SamplesPerBlock;
            public ushort ADPCM;
            public byte[] ADPCMCoefSets; //28
        }

        private struct WAVEFORMATEX
            {
            public ushort wFormatTag;
            public ushort nChannels;
            public uint nSamplesPerSec;
            public uint nAvgBytesPerSec;
            public ushort nBlockAlign;
            public ushort wBitsPerSample;
            public ushort cbSize;
        }

        private static SoundEntry[] soundEntries;
        public static int soundEntriesCount;


        public const int S_OK = 0x00000000;


        internal static void DEBUG()
        {
            string pt = Path.Combine(Memory.FF8DIR , "../Music/dmusic/");
            Memory.musices = Directory.GetFiles(pt, "*.sgt");
            //PlayMusic();
            //FileStream fs = new FileStream(pt, FileMode.Open, FileAccess.Read);
            //BinaryReader br = new BinaryReader(fs);
            //string RIFF = Encoding.ASCII.GetString(br.ReadBytes(4));
            //if (RIFF != "RIFF") throw new Exception("NewDirectMusic::NOT RIFF");
            //uint eof = br.ReadUInt32();
            //if (fs.Length != eof + 8) throw new Exception("NewDirectMusic::RIFF length/size indicator error");
            //string dmsg = Encoding.ASCII.GetString(br.ReadBytes(4));
            //var SegmentHeader = GetSGTSection(br);
            //var guid = GetSGTSection(br);
            //var list = GetSGTSection(br);
            //var vers = GetSGTSection(br);
            //var list_unfo = GetSGTSection(br);
            //string UNFO = SGT_ReadUNAM(list_unfo).TrimEnd('\0');
            //var trackList = GetSGTSection(br);
            //List<byte[]> Tracks = ProcessTrackList(trackList.Item2);
            //byte[] sequenceTrack = Tracks[2];
            //PseudoBufferedStream pbs = new PseudoBufferedStream(sequenceTrack);
            //pbs.Seek(44, PseudoBufferedStream.SEEK_BEGIN);
            //string seqt = Encoding.ASCII.GetString(BitConverter.GetBytes(pbs.ReadUInt()));
        }

        internal static void DEBUG_SoundAudio()
        {
            string path = Path.GetFullPath(Path.Combine(Memory.FF8DIR, "../Sound/audio.fmt"));
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                soundEntries = new SoundEntry[br.ReadUInt32()];
                fs.Seek(36, SeekOrigin.Current);
                for (int i = 0; i < soundEntries.Length-1; i++)
                {
                    int sz = br.ReadInt32();
                    if(sz == 0) {
                        fs.Seek(34, SeekOrigin.Current); continue; }

                    soundEntries[i] = new SoundEntry()
                    {
                        Size = sz,
                        Offset = br.ReadInt32(),
                        UNK = br.ReadBytes(12),
                        WAVFORMATEX = br.ReadBytes(18),
                        SamplesPerBlock = br.ReadUInt16(),
                        ADPCM = br.ReadUInt16(),
                        ADPCMCoefSets = br.ReadBytes(28)
                    };
                }
            }
            soundEntriesCount = soundEntries.Length;
        }

        internal static void PlaySound(int soundID)
        {
            if (soundEntries == null)
                return;
            if (soundEntries[soundID].Size == 0) return;


            using (FileStream fs = new FileStream(MakiExtended.GetUnixFullPath( Path.Combine(Memory.FF8DIR, "..\\Sound\\audio.dat")), FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                fs.Seek(soundEntries[soundID].Offset, SeekOrigin.Begin);
                //List<byte[]> sfxBufferList = new List<byte[]>();
                //sfxBufferList.Add(Encoding.ASCII.GetBytes("RIFF"));
                //sfxBufferList.Add(BitConverter.GetBytes
                //    (soundEntries[soundID].Size + 36));
                //sfxBufferList.Add(Encoding.ASCII.GetBytes("WAVEfmt "));
                //sfxBufferList.Add(BitConverter.GetBytes
                //    (18 + 0));
                //sfxBufferList.Add(soundEntries[soundID].WAVFORMATEX);
                //sfxBufferList.Add(Encoding.ASCII.GetBytes("data"));
                //sfxBufferList.Add(BitConverter.GetBytes(soundEntries[soundID].Size));
                GCHandle gc = GCHandle.Alloc(soundEntries[soundID].WAVFORMATEX, GCHandleType.Pinned);
                WAVEFORMATEX format =  (WAVEFORMATEX)Marshal.PtrToStructure(gc.AddrOfPinnedObject(), typeof(WAVEFORMATEX));
                gc.Free();
                byte[] rawBuffer = br.ReadBytes(soundEntries[soundID].Size);
                //sfxBufferList.Add(rawBuffer);
                //byte[] sfxBuffer = sfxBufferList.SelectMany(x => x).ToArray();


                //WaveFileReader rad = new WaveFileReader(new MemoryStream(sfxBuffer));
                //passing WAVEFORMATEX struct params makes playing all sounds possible
#if _WINDOWS
                RawSourceWaveStream raw = new RawSourceWaveStream(new MemoryStream(rawBuffer), new AdpcmWaveFormat((int)format.nSamplesPerSec, format.nChannels ));
                var a = WaveFormatConversionStream.CreatePcmStream(raw);
                WaveOut waveout = new WaveOut();
                waveout.Init(a);
                waveout.Play();
#else
                SoundEffect se = new SoundEffect(rawBuffer, (int)format.nSamplesPerSec/2, (AudioChannels)format.nChannels);
                se.Play();
#endif


                //libZPlay.ZPlay zplay = new libZPlay.ZPlay();

                //zplay.OpenFile("D:\\test.wav", libZPlay.TStreamFormat.sfAutodetect);
                //zplay.StartPlayback();
                //SoundEffect se = new SoundEffect(sfxBuffer, 22050, AudioChannels.Mono);
                //sei.Play();
                //se.Play(1.0f, 0.0f, 0.0f);
                //se.Dispose();
            }
        }

        public static void StopSound()
        {
            //waveout.Stop();
        }

        internal static void update()
        {

        }
        //callable test
        unsafe public static void PlayMusic()
        {
#if _WINDOWS
            if (Memory.musicIndex >= Memory.musices.Length)
            {
                Memory.musicIndex--;
                return;
            }
            string pt = Memory.musices[Memory.musicIndex];
            if (cdm == null)
            {
                cdm = new CDirectMusic();
                cdm.Initialize();
                loader = new CDLSLoader();
                loader.Initialize();
                loader.LoadSegment(pt, out segment);
                ccollection = new CCollection();
                loader.LoadDLS(Memory.FF8DIR + "/../Music/dmusic/FF8.dls", out ccollection);
                uint dwInstrumentIndex = 0;
                INSTRUMENTINFO iInfo;
                while (ccollection.EnumInstrument(++dwInstrumentIndex, out iInfo) == S_OK)
                {
                    Debug.WriteLine(iInfo.szInstName);
                }
                instruments = new CInstrument[dwInstrumentIndex];

                path = new CAPathPerformance();
                path.Initialize(cdm, null, null, DMUS_APATH.DYNAMIC_3D, 128);
                cport = new CPortPerformance();
                cport.Initialize(cdm, null, null);
                outport = new COutputPort();
                outport.Initialize(cdm);

                uint dwPortCount = 0;
                INFOPORT infoport;
                do
                    outport.GetPortInfo(++dwPortCount, out infoport);
                while ((infoport.dwFlags & DMUS_PC.SOFTWARESYNTH) == 0);

                outport.SetPortParams(0, 0, 0, SET.REVERB | SET.CHORUS, 44100);
                outport.ActivatePort(infoport);

                cport.AddPort(outport, 0, 1);

                for (int i = 0; i < dwInstrumentIndex; i++)
                {
                    ccollection.GetInstrument(out instruments[i], i);
                    outport.DownloadInstrument(instruments[i]);
                }
                segment.Download(cport);
                cport.PlaySegment(segment);
            }
            else
            {
                cport.Stop(segment);
                segment.Dispose();
                //segment.ConnectToDLS
                loader.LoadSegment(pt, out segment);
                segment.Download(cport);
                cport.PlaySegment(segment);
                cdm.Dispose();
            }
            
            //GCHandle.Alloc(cdm, GCHandleType.Pinned);
            //GCHandle.Alloc(loader, GCHandleType.Pinned);
            //GCHandle.Alloc(segment, GCHandleType.Pinned);
            //GCHandle.Alloc(path, GCHandleType.Pinned);
            //GCHandle.Alloc(cport, GCHandleType.Pinned);
            //GCHandle.Alloc(outport, GCHandleType.Pinned);
            //GCHandle.Alloc(infoport, GCHandleType.Pinned);
#endif
            
        }

        public static void KillAudio()
        {
#if _WINDOWS
            try
            {
                cport.StopAll();
                cport.Dispose();
                ccollection.Dispose();
                loader.Dispose();
                outport.Dispose();
                path.Dispose();
                cdm.Dispose();
            }
            catch
            {
                ;
            }
#endif
    }

        public static void StopAudio()
        {
#if _WINDOWS
            cport.StopAll();
#endif
        }

        private static List<byte[]> ProcessTrackList(byte[] item2)
        {
            PseudoBufferedStream pbs = new PseudoBufferedStream(item2);
            string head = Encoding.ASCII.GetString(BitConverter.GetBytes(pbs.ReadUInt()));

            head = Encoding.ASCII.GetString(BitConverter.GetBytes(pbs.ReadUInt())); //RIFF start
            List<byte[]> Tracks = new List<byte[]>();
            //now process tracks
            while (head == "RIFF")
            {
                uint vara = pbs.ReadUInt();
                byte[] buf = pbs.ReadBytes(vara);
                Tracks.Add(buf);
                if (pbs.Tell() == pbs.Length)
                    break;
                head = Encoding.ASCII.GetString(BitConverter.GetBytes(pbs.ReadUInt()));
            }
            return Tracks;
        }

        private static string SGT_ReadUNAM(Tuple<string, byte[]> uNFO)
        {
            PseudoBufferedStream pbs = new PseudoBufferedStream(uNFO.Item2);
            string unfo = Encoding.ASCII.GetString(BitConverter.GetBytes(pbs.ReadUInt()));
            string unam = Encoding.ASCII.GetString(BitConverter.GetBytes(pbs.ReadUInt()));
            uint vara = pbs.ReadUInt();
            return Encoding.Unicode.GetString(pbs.ReadBytes(vara));
        }

        private static Tuple<string, byte[]> GetSGTSection_pbs(byte[] buffer)
        {
            PseudoBufferedStream pbs = new PseudoBufferedStream(buffer);
            string head = Encoding.ASCII.GetString(BitConverter.GetBytes(pbs.ReadUInt()));
            uint vara = pbs.ReadUInt();
            byte[] buf = pbs.ReadBytes(vara);
            return new Tuple<string, byte[]>(head, buf);
        }

        private static Tuple<string, byte[]> GetSGTSection(BinaryReader br)
        {
            string head = Encoding.ASCII.GetString(br.ReadBytes(4));
            uint vara = br.ReadUInt32();
            byte[] segData = br.ReadBytes((int)vara);
            return new Tuple<string, byte[]>(head, segData);
        }


    }
}
